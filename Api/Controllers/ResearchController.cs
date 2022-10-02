using Microsoft.AspNetCore.Mvc;
using PSSN.Common;
using PSSN.Common.Requests;
namespace PSSN.Api.Controllers;

[Controller]
[Route("api/v1/[controller]")]
public class ResearchController : ControllerBase
{
    private readonly IServiceCollection _serviceDescriptors;

    public ResearchController(IServiceCollection serviceDescriptors)
    {
        this._serviceDescriptors = serviceDescriptors;
    }
    [HttpGet]
    [Route("simple")]
    public async Task<ActionResult<IEnumerable<VectorResponse>>> ResearchAsync([FromBody] SimpleResearchRequest request){
        return Ok(new{ Message = "ok!", K = request.K, A = request.A});
    }
}