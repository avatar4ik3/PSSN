using Microsoft.AspNetCore.Mvc;
using PSSN.Common;
using PSSN.Common.Requests;
using PSSN.Core;
using PSSN.Core.Matrices;
using PSSN.Core.Operators;
using PSSN.Core.Strategies;

namespace PSSN.Api.Controllers;

[Controller]
[Route("api/v1/[controller]")]
public class ResearchController : ControllerBase
{
    private readonly StrategesContainer _container;
    private readonly PopulationFrequency _researcher;

    public ResearchController(StrategesContainer container, PopulationFrequency researcher)
    {
        this._container = container;
        this._researcher = researcher;
    }
    [HttpGet]
    [Route("simple")]
    public async Task<ActionResult<IEnumerable<VectorResponse>>> ResearchAsync(int k, int r, string[] strats, double[][] po)
    {
        try
        {
            var strategies = strats.Select(s => _container[s]);
            return Ok(await RunGamePhase(k, r, strategies, po));
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    private async Task<IEnumerable<VectorResponse>> RunGamePhase(int k, int r, IEnumerable<IStrategy> strats, double[][] po)
    {
        var strategies = strats.ToArray();
        var innerResult = _researcher.Research(k, strategies, po);
        List<VectorResponse> response = new List<VectorResponse>();
        for (int i = 0; i < k; ++i)
        {
            var vector = new VectorResponse() { Ki = i };
            for (int s = 0; s < strats.Count(); ++s)
            {
                vector.Values.Add(strategies[s].Name, innerResult[i][s]);
            }
            response.Add(vector);
        }
        return response;
    }

    [HttpGet]
    [Route("simple")]
    public async Task<ActionResult<GenerationResponse>> ResearchGenerationAsync([FromQuery] GenerationRequest request)
    {
        try
        {
            var gen = new FilledStrategiesGenerator(
                request.Population,
                new SingleFilledStrategyGenerator(request.R, new Random()));
            var strats = gen.Generate().ToList();
            var result = new GenerationResponse();
            foreach (var i in ..request.K)
            {
                result.items.Add(new Item
                {
                    strats = strats,
                    response = await RunGamePhase(request.k, request.R, strats, request.Ro)
                });
                // var selection = new SelectionOperator(3);
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}