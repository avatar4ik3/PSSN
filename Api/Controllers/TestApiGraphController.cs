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
        return Ok(_mapper.Map<IEnumerable<VectorResponse>>(await _repository.GetVectors()));
    }
}