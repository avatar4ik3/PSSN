using System.Xml;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PSSN.Api.Profiles;
using PSSN.Common.Requests;
using PSSN.Core.Round;

namespace PSSN.Benchmarks;
[MemoryDiagnoser]
[Config(typeof(AntiVirusFriendlyConfig))]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class HardEndpointBenchmark
{
    private GenerationRequest _req = new GenerationRequest()
    {
        Ro = new[]
        {
            new double[] {4, 0},
            new double[] {6, 1}
        },
        GenCount = 6,
        Population = 50,
        SelectionGoupSize = 4,
        CrossingCount = 3,
        SwapChance = 0.4
    };

    private Random _random = new Random();
    private IGameRunner _runner = new SimpleGameRunner();
    private Mapper _mapper = new Mapper(new MapperConfiguration(options => options.AddProfile<VectorProfile>()));

    [Benchmark]
    public void Default()
    {
        var data = HardEndpointImps.Solution_Default(_req, _mapper, _runner, _random);
    }
    
    [Benchmark]
    public void NoToList()
    {
        var data = HardEndpointImps.Solution_NoCopy_NoToList_NoBadAlloc(_req, _mapper, _runner, _random);
    }

    //[Benchmark]
    public void NoMapper()
    {
        var data = HardEndpointImps.Solution_NoCopy_NoToList_NoBadAlloc(_req, _mapper, _runner, _random);
    }
}