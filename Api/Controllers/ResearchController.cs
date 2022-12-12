using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSSN.Common;
using PSSN.Common.Model;
using PSSN.Common.Requests;
using PSSN.Core;
using PSSN.Core.Matrices;
using PSSN.Core.Operators;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Api.Controllers;

[Controller]
[Route("api/v1/[controller]")]
public class ResearchController : ControllerBase
{
    private readonly StrategesContainer _container;
    private readonly PopulationFrequency _researcher;
    private readonly Random _random;
    private readonly IGameRunner _gameRunner;
    private readonly IMapper _mapper;

    public ResearchController(StrategesContainer container, PopulationFrequency researcher, Random random, IGameRunner gameRunner, IMapper mapper)
    {
        this._container = container;
        this._researcher = researcher;
        this._random = random;
        this._gameRunner = gameRunner;
        this._mapper = mapper;
    }
    [HttpGet]
    [Route("simple")]
    public async Task<ActionResult<IEnumerable<VectorResponse>>> ResearchAsync(int k, int r, string[] strats, double[][] po)
    {
        try
        {
            var strategies = strats.Select(s => _container[s]);
            return Ok((await RunGamePhase(k, r, strategies, po)).Matrix);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    private async Task<(IEnumerable<VectorResponse> Matrix, TreeGameRunnerResult Tree)> RunGamePhase(int k, int r, IEnumerable<IStrategy> strats, double[][] po)
    {
        var strategies = strats.ToArray();
        var innerResult = _researcher.Research(k, r, strategies, po);
        List<VectorResponse> response = new List<VectorResponse>();
        for (int i = 0; i < k; ++i)
        {
            var vector = new VectorResponse() { Ki = i };
            for (int s = 0; s < strats.Count(); ++s)
            {
                vector.Values.Add(strategies[s].Name, innerResult.Vectors[i][s]);
            }
            response.Add(vector);
        }
        return new(response, innerResult.Tree);
    }

    [HttpGet]
    [Route("hard")]
    public async Task<ActionResult<GenerationResponse>> ResearchGenerationAsync([FromQuery] GenerationRequest request)
    {
        // try
        // {
        request.Ro = new double[][]{
                new double []{4,0},new double[]{6,1}};
        var gen = new FilledStrategiesGenerator(
            request.Population,
            new SingleFilledStrategyGenerator(request.GenCount, _random));
        var strats = gen.Generate().ToList();
        var result = new GenerationResponse();
        foreach (var i in ..request.Population)
        {
            var tree = _gameRunner.Play(strats, request.Ro, request.GenCount);

            var m_s = _mapper.Map<List<FilledStrategyModel>>(strats.Copy().ToList());
            var m_t = _mapper.Map<ResultTree>(tree);

            result.Items.Add(new Item(m_s, m_t));
            var newPopulation = new List<FilledStrategy>();

            //preparetion of new population
            var selectionOperator = new SelectionOperator(request.SelectionGoupSize, _random, tree);
            var crossingOverOperator =
                                    new BestScorePickerCrossingOverOperator(
                                    request.CrossingCount, strats, tree);
            var mutationOperator = new MutationOperator(request.SwapChance, _random);

            foreach (var j in ..((strats.Count / 2) - 1))
            {
                var s1 = (FilledStrategy)selectionOperator.Operate(strats);
                var s2 = (FilledStrategy)selectionOperator.Operate(strats);



                var crossovers = crossingOverOperator.Operate(s1, s2);

                var mutated = mutationOperator.Operate(crossovers.Cast<FilledStrategy>());


                newPopulation.AddRange(mutated.Cast<FilledStrategy>());
            }
            strats = newPopulation.Copy()
                .Zip(Enumerable.Range(0, newPopulation.Count()))
                .Select(x => new FilledStrategy(x.First.behaviours, x.Second.ToString()))
                .ToList();
        }
        return Ok(result);
        // }
        // catch (Exception e)
        // {
        //     return BadRequest(e);
        // }
    }
}