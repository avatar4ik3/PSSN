using AutoMapper;
using BenchmarkDotNet.Running;
using PSSN.Api.Profiles;
using PSSN.Common.Requests;
using PSSN.Core.Round;

namespace PSSN.Benchmarks;

public class Program
{
    static void Main(string[] args)
    {
        GenerationRequest _req = new GenerationRequest()
        {
            Ro = new[]
            {
                new double[] {4, 0},
                new double[] {6, 1}
            },
            GenCount = 6,
            Population = 100,
            SelectionGoupSize = 10,
            CrossingCount = 3,
            SwapChance = 0.4
        };

        Random _random = new Random();
        IGameRunner _runner = new SimpleGameRunner();
        Mapper _mapper = new Mapper(new MapperConfiguration(options => options.AddProfile<VectorProfile>()));
        
        
        var data = HardEndpointImps.Solution_Default(_req, _mapper, _runner, _random);

        var a = 2;
        Console.WriteLine("a");

        // BenchmarkRunner.Run<HardEndpointBenchmark>();
    }
}