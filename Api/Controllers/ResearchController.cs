using Microsoft.AspNetCore.Mvc;
using PSSN.Common;
using PSSN.Common.Requests;
using PSSN.Core;
using PSSN.Core.Matrices;

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
            var strategies = strats.Select(s => _container[s]).ToArray();
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
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest();
        }



    }
}