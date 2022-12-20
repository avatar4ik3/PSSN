using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using PSSN.Api.ServiceInterfaces;
using PSSN.Common;
using PSSN.Common.Responses;

namespace PSSN.Api.Controllers;

[ApiController]
[Route("api/v1/")]
public class TestApiGraphController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IParserService _repository;

    public TestApiGraphController(IParserService repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("test")]
    public async Task<ActionResult<IEnumerable<VectorResponse>>> GetVectorsAsync()
    {
        var source = await _repository.GetVectors();
        Console.WriteLine("Done calculus");
        var value = _mapper.Map<IEnumerable<VectorResponse>>(source);
        Console.WriteLine("Done mapping");
        return base.Ok(value);
    }
}