using Microsoft.AspNetCore.Mvc;
using PSSN.Common;

namespace PSSN.Api.Controllers;

public class TestApiGraphController : ControllerBase
{
    public TestApiGraphController()
    {
        
    }

    [HttpGet]
    public ActionResult<IEnumerable<VectorResponse>> GetVectors(){
        return Ok();
    }
}