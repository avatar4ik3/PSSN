using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSSN.Common.Model;
using PSSN.Core;
using PSSN.Core.Containers;
using PSSN.Core.Generators;
using PSSN.Core.Operators;
using PSSN.Core.Operators.MemeOperators;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Api.Controllers;

[Controller]
[Route("api/v1/[controller]")]
public class MemesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly PatternsContainer _patternsContainer;
    private readonly IGameRunner _gameRunner;
    private readonly Random _random;

    public MemesController(
        IMapper mapper,
        PatternsContainer patternsContainer,
        IGameRunner gameRunner,
        Random random)
    {
        this._mapper = mapper;
        this._patternsContainer = patternsContainer;
        this._gameRunner = gameRunner;
        this._random = random;
    }
    

    [HttpPost]
    [Route("generate")]
    public ActionResult GenerateRandom(GenerateMemeRequestModel model)
    {
        Random random;
        if (model.RandomSeed is not null)
        {
            random = new Random(model.RandomSeed.Value);
        }
        else
        {
            random = new Random();
        }
        var res = ConditionalStrategyBuilder.RandomMemes(random,model.Distr,model.Count,model.GenotypeSize,_patternsContainer).ToList();
        return Ok(_mapper.Map<List<MemeStrategyModel>>(res));
    }

    [HttpPost]
    [Route("research-single")]
    public ActionResult ResearchSingleGeneration(MemeSingleGeneratinoRequestModel model){
        var strats = _mapper.Map<List<ConditionalStrategy>>(model.Models,opt => opt.AfterMap((a,b) => {
            foreach(var (dest, src) in b.Zip(model.Models)){
                dest.Patterns = new(){
                    _patternsContainer.CreatePattern(src.Type,src.Coefs)
                };
            }
        })).ToArray();
        var tree = _gameRunner.Play(strats,model.Payofss,model.GenCount);

        var selectionOperator = new SelectionOperator<ConditionalStrategy>(model.SelectionGroupSize, tree, _random);
        var crossingOverOperator = new MemeCrossingOverOperator(strats,tree);
        var mutationOperator = new MemeMutationOperator(model.SwapChance,_random);


        var newPopulation = new List<ConditionalStrategy>();

        foreach (var _ in ..(strats.Length / 2 + strats.Length % 2))
        {
            var s1 = selectionOperator.Operate(strats);
            var s2 = selectionOperator.Operate(strats);

            var crossovers = crossingOverOperator.Operate(s1, s2);

            var mutated = mutationOperator.Operate(crossovers);

            newPopulation.AddRange(mutated);
        }

        var response = new MemeSingleGenerationResponseModel()
        {
            GameResult = new MemeGenerationResponseModel(){
                Strats = _mapper.Map<List<MemeStrategyModel>>(strats),
                Result = _mapper.Map<ResultTree>(tree)
            },
            NewStrats = _mapper.Map<List<MemeStrategyModel>>(newPopulation)
        };
        return Ok(response);
    }
}


public class MemeSingleGenerationResponseModel{
    public List<MemeStrategyModel> NewStrats { get; set; }
    public MemeGenerationResponseModel GameResult { get; set; }
}
public class MemeGenerationResponseModel {
    public List<MemeStrategyModel> Strats { get; set; }
    public ResultTree Result {get;set;} 
    
}
public class MemeSingleGeneratinoRequestModel{
    public List<MemeStrategyModel> Models { get; set; }
    public double[][] Payofss {get;set;}
    public int GenCount {get;set;}
    public int SelectionGroupSize { get; set; }
    public double SwapChance { get; set; }
}

public class MemeStrategyModel{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Behavior>? Behaviors { get; set; }
    public string Type {get;set;}
    public int[] Coefs {get;set;}
}

public class GenerateMemeRequestModel
{
    public double Distr { get; set; }
    public int Count { get; set; }
    public int GenotypeSize { get; set; }
    public int? RandomSeed { get; set; }
}