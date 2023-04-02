using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSSN.Common.Model;
using PSSN.Common.Requests;
using PSSN.Common.Responses;
using PSSN.Core;
using PSSN.Core.Containers;
using PSSN.Core.Generators;
using PSSN.Core.Matricies;
using PSSN.Core.Operators;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
public class ResearchController : ControllerBase
{
    private readonly PatternsContainer _patternsContainer;
    private readonly IGameRunner _gameRunner;
    private readonly IMapper _mapper;
    private readonly Random _random;
    private readonly PopulationFrequency _researcher;

    public ResearchController(PatternsContainer patternsContainer, PopulationFrequency researcher,
        IGameRunner gameRunner, IMapper mapper, Random random)
    {
        this._patternsContainer = patternsContainer;
        _researcher = researcher;
        _gameRunner = gameRunner;
        _mapper = mapper;
        this._random = random;
    }

    [HttpPost]
    [Route("simple")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VectorResponse>))]
    public ActionResult<IEnumerable<VectorResponse>> Research(SimpleResearchRequest request)
    {
        var strategies = _mapper.Map<List<ConditionalStrategy>>(request.Strats).Zip(request.Strats).Select(x =>
        {
            x.First.Patterns = x.Second.Patterns.Select(y => _patternsContainer.CreatePattern(y.Name!, y.Coeffs!)).ToList();
            return x.First;
        }).ToArray();

        var innerResult = _researcher.Research(request.K, request.R, strategies, request.Po!).Vectors;
        var response = new List<VectorResponse>();
        for (var i = 0; i < request.K; ++i)
        {
            var vector = new VectorResponse { Ki = i };
            for (var s = 0; s < request.Strats.Count(); ++s) vector.Values.Add(strategies[s].Name, innerResult[i][s]);
            response.Add(vector);
        }

        return Ok(response);
    }

    [HttpGet]
    [Route("generate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConditionalStrategyModel>))]
    public ActionResult<IEnumerable<ConditionalStrategyModel>> GenerateStrategies([FromQuery] GenerateStrategiesRequest request)
    {
        var builder = new ConditionalStrategyBuilder(_random);
        var strats = new List<ConditionalStrategy>(request.Count);

        foreach (var i in ..request.Count)
        {
            strats.Add(
                builder
                .WithName(i.ToString())
                .WithRandomBehaviours(request.GenCount, request.DistributionChance)
                .Build());
        }

        return Ok(_mapper.Map<List<ConditionalStrategyModel>>(strats));
    }

    [HttpPost]
    [Route("split")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleGenerationResponse))]
    public ActionResult<SingleGenerationResponse> ResearchSingleGeneration(SingleGenerationRequest request)
    {
        var strats = _mapper.Map<List<ConditionalStrategy>>(request.Strats).Zip(request.Strats).Select(x =>
       {
           if (x.Second.Patterns is not null)
           {
               x.First.Patterns = x.Second.Patterns.ConvertAll(y => _patternsContainer.CreatePattern(y.Name!, y.Coeffs!));
           }
           return x.First;
       }).ToArray();
        var tree = _gameRunner.Play(strats, request.Ro!, request.GenCount);

        var newPopulation = new List<ConditionalStrategy>();

        var selectionOperator = new SelectionOperator<ConditionalStrategy>(request.SelectionGroupSize, tree, _random);
        var crossingOverOperator =
            new BestScorePickerCrossingOverOperator(
                request.CrossingCount, strats, tree);

        var mutationOperator = new MutationOperator(request.SwapChance, _random);
        foreach (var _ in ..(strats.Length / 2 + strats.Length % 2))
        {
            var s1 = selectionOperator.Operate(strats);
            var s2 = selectionOperator.Operate(strats);

            var crossovers = crossingOverOperator.Operate(s1, s2);

            var mutated = mutationOperator.Operate(crossovers);

            newPopulation.AddRange(mutated);
        }

        var response = new SingleGenerationResponse()
        {
            GameResult = new GenerationResponseItem(
                _mapper.Map<List<ConditionalStrategyModel>>(strats),
                _mapper.Map<ResultTree>(tree)
            ),
            NewStrats = _mapper.Map<List<ConditionalStrategyModel>>(newPopulation.Zip(Enumerable.Range(0, newPopulation.Count)).Select(x =>
            {
                // if(x.First.Name[0] is 'C' or 'D' or 'R'){
                    // var l = x.First.Name.IndexOfAny(new[]{'0','1','2','3','4','5','6','7','8','9'});
                    // x.First.Name = x.First.Name.Substring(0,l);
                    // x.First.Name += x.Second.ToString();
                // }
                // else{
                    x.First.Name = x.Second.ToString();
                // }
                return x.First;
            }))
        };

        return Ok(response);
    }

    public record AgainsR(List<ConditionalStrategyModel> Strats, int K_repeated, double[][] A);
    [HttpPost]
    [Route("against")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenerationResponseItem))]
    public ActionResult<GenerationResponseItem> PlayAgainst(AgainsR param)
    {
        var strategies = _mapper.Map<List<ConditionalStrategy>>(param.Strats).Zip(param.Strats).Select(x =>
        {
            x.First.Patterns = x.Second.Patterns.ConvertAll(y => _patternsContainer.CreatePattern(y.Name!, y.Coeffs!));
            return x.First;
        }).ToArray();
        var tree = _gameRunner.Play(strategies, param.A, param.K_repeated);
        return Ok(new GenerationResponseItem(
            null!,
            _mapper.Map<ResultTree>(tree)
        ));
    }
}