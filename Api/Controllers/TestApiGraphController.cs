using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSSN.Api.Data;
using PSSN.Common;

namespace PSSN.Api.Controllers;

[ApiController]
[Route("api/v1/")]
public class TestApiGraphController : ControllerBase
{
    private readonly ITestRepository _repository;
    private readonly IMapper _mapper;

    public TestApiGraphController(ITestRepository repository, IMapper mapper)
    {
        this._repository = repository;
        this._mapper = mapper;
    }

    [HttpGet("test")]
    public async Task<ActionResult<IEnumerable<VectorResponse>>> GetVectorsAsync(){
        IEnumerable<Model.ResultVector> source = await _repository.GetVectors();
        Console.WriteLine("Done calculus");
        IEnumerable<VectorResponse> value = _mapper.Map<IEnumerable<VectorResponse>>(source);
        Console.WriteLine("Done mapping");
        return base.Ok(value);
    }
}