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
    private readonly PopulationFrequency _researcher;

    public ResearchController(StrategiesContainer container, PopulationFrequency researcher,
        IGameRunner gameRunner, IMapper mapper)
    {
        _container = container;
        _researcher = researcher;
        _gameRunner = gameRunner;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("simple")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VectorResponse>))]
    public ActionResult<IEnumerable<VectorResponse>> Research([FromQuery] int k, [FromQuery] int r, [FromQuery] string[] strats, [FromQuery] double[][] po)
    {

        var strategies = strats.Select(s => _container[s]).ToArray();
        var innerResult = _researcher.Research(k, 6, strategies, po).Vectors;
        var response = new List<VectorResponse>();
        for (var i = 0; i < k; ++i)
        {
            var vector = new VectorResponse { Ki = i };
            for (var s = 0; s < strats.Length; ++s) vector.Values.Add(strategies[s].Name, innerResult[i][s]);
            response.Add(vector);
        }

        return Ok(response);
    }

    [HttpGet]
    [Route("hard")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FilledStrategyModel>))]
    public ActionResult<GenerationResponse> ResearchGeneration([FromQuery] GenerationRequest request)
    {
        request.Ro = new[]
        {
            new double[] {4, 0}, new double[] {6, 1}
        };

        var gen = new FilledStrategiesGenerator(
            request.Population,
            new SingleFilledStrategyGenerator(request.GenCount));

        var strats = gen
            .Generate()
            .ToList();

        var result = new GenerationResponse();

        foreach (var _ in ..request.Population)
        {
            var tree = _gameRunner.Play(strats, request.Ro, request.GenCount);

            var m_s = _mapper.Map<List<FilledStrategyModel>>(strats.Copy().ToList());
            var m_t = _mapper.Map<ResultTree>(tree);

            result.Items.Add(new Item(m_s, m_t));
            var newPopulation = new List<FilledStrategy>();

            var selectionOperator = new SelectionOperator(request.SelectionGoupSize, tree);
            var crossingOverOperator =
                new BestScorePickerCrossingOverOperator(
                    request.CrossingCount, strats, tree);
            var mutationOperator = new MutationOperator(request.SwapChance);

            foreach (var __ in ..(strats.Count / 2 - 1))
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
    }
}