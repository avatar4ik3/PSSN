using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSSN.Common.Model;
using PSSN.Common.Requests;
using PSSN.Common.Responses;
using PSSN.Core;
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
    private readonly StrategiesContainer _container;
    private readonly IGameRunner _gameRunner;
    private readonly IMapper _mapper;
    private readonly Random _random;
    private readonly PopulationFrequency _researcher;

    public ResearchController(StrategiesContainer container, PopulationFrequency researcher,
        IGameRunner gameRunner, IMapper mapper, Random random)
    {
        _container = container;
        _researcher = researcher;
        _gameRunner = gameRunner;
        _mapper = mapper;
        this._random = random;
    }

    [HttpGet]
    [Route("simple")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VectorResponse>))]
    public ActionResult<IEnumerable<VectorResponse>> Research([FromQuery] SimpleResearchRequest request)
    {
        var strategies = request.Strats.Select(s => _container[s]).ToArray();
        var innerResult = _researcher.Research(request.K, request.R, strategies, request.Po).Vectors;
        var response = new List<VectorResponse>();
        for (var i = 0; i < request.K; ++i)
        {
            var vector = new VectorResponse { Ki = i };
            for (var s = 0; s < request.Strats.Count(); ++s) vector.Values.Add(strategies[s].Name, innerResult[i][s]);
            response.Add(vector);
        }

        return Ok(response);
    }

    //примерная сложность p^3 
    [HttpGet]
    [Route("hard")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenerationResponse))]
    public ActionResult<GenerationResponse> ResearchGeneration([FromQuery] GenerationRequest request)
    {
        var gen = new FilledStrategiesGenerator(
            request.Population,
            new SingleFilledStrategyGenerator(request.GenCount, _random));

        var strats = gen
            .Generate()
            .ToList();

        var result = new GenerationResponse();

        foreach (var _ in ..request.Population)
        {
            //C_Count_2_Population * GenCount ::::: <- GenCount это GPR 
            var tree = _gameRunner.Play(strats, request.Ro!, request.GenCount);

            result.Items.Add(new(
                _mapper.Map<List<FilledStrategyModel>>(strats.Copy().ToList()),
                _mapper.Map<ResultTree>(tree)));

            var newPopulation = new List<FilledStrategy>();

            var selectionOperator = new SelectionOperator(request.SelectionGoupSize, tree, _random);
            var crossingOverOperator =
                new BestScorePickerCrossingOverOperator(
                    request.CrossingCount, strats, tree);
            var mutationOperator = new MutationOperator(request.SwapChance, _random);
            //INNER_C = Population / 2 + 1 если Populations нечетное
            foreach (var __ in ..(strats.Count / 2 + strats.Count % 2))
            {
                //INNER_C 
                var s1 = selectionOperator.Operate(strats);
                var s2 = selectionOperator.Operate(strats);

                var crossovers = crossingOverOperator.Operate(s1, s2);

                //INNER_C * Population
                var mutated = mutationOperator.Operate(crossovers);

                newPopulation.AddRange(mutated);
            }

            strats = newPopulation.Copy()
                .Zip(Enumerable.Range(0, newPopulation.Count()))
                .Select(x => new FilledStrategy(x.First.behaviours, x.Second.ToString()))
                .ToList();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("generate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FilledStrategyModel>))]
    public ActionResult<IEnumerable<FilledStrategyModel>> GenerateStrategies([FromQuery] GenerateStrategiesRequest request)
    {
        var gen = new FilledStrategiesGenerator(
           request.Count,
           new SingleFilledStrategyGenerator(request.GenCount, _random));

        var strats = gen.Generate();

        return Ok(_mapper.Map<List<FilledStrategyModel>>(strats));
    }

    [HttpPost]
    [Route("split")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleGenerationResponse))]
    public ActionResult<SingleGenerationResponse> ResearchSingleGeneration(SingleGenerationRequest request)
    {
        var strats = _mapper.Map<List<FilledStrategy>>(request.Strats).Copy().ToList();
        var tree = _gameRunner.Play(strats, request.Ro!, request.GenCount);

        var newPopulation = new List<FilledStrategy>();

        var selectionOperator = new SelectionOperator(request.SelectionGroupSize, tree, _random);
        var crossingOverOperator =
            new BestScorePickerCrossingOverOperator(
                request.CrossingCount, strats, tree);

        var mutationOperator = new MutationOperator(request.SwapChance, _random);
        foreach (var _ in ..(strats.Count / 2 + strats.Count % 2))
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
                _mapper.Map<List<FilledStrategyModel>>(strats),
                _mapper.Map<ResultTree>(tree)
            ),
            NewStrats = _mapper.Map<List<FilledStrategyModel>>(newPopulation.Zip(Enumerable.Range(0, newPopulation.Count)).Select(x =>
            {
                x.First.Name = x.Second.ToString();
                return x.First;
            }))
        };

        return Ok(response);
    }
}