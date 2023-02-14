using AutoMapper;

using PSSN.Api.Model;
using PSSN.Api.Profiles;
using PSSN.Common.Model;
using PSSN.Common.Responses;
using PSSN.Core;
using PSSN.Core.Matricies;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Api.Tests;

public class VectorMapperTets
{
    private readonly IMapper _mapper;

    public VectorMapperTets()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<VectorProfile>());
        _mapper = new Mapper(mapperConfig);
    }

    // [Fact]
    // public void Mapper_MapsDictionary()
    // {
    //     var vector = new ResultVector
    //     {
    //         Stage = 0,
    //         Vector = new Dictionary<IStrategy, double>
    //         {
    //             {new EmptyStrategy {Name = "C"}, 0.125},
    //             {new EmptyStrategy {Name = "CTT"}, 0.125},
    //             {new EmptyStrategy {Name = "CRTT"}, 0.125},
    //             {new EmptyStrategy {Name = "CD"}, 0.125},
    //             {new EmptyStrategy {Name = "DC"}, 0.125},
    //             {new EmptyStrategy {Name = "DTT"}, 0.125},
    //             {new EmptyStrategy {Name = "DRTT"}, 0.125},
    //             {new EmptyStrategy {Name = "DD"}, 0.125}
    //         }
    //     };
    //
    //     var actual = _mapper.Map<VectorResponse>(vector);
    //
    //     var expected = new VectorResponse
    //     {
    //         Ki = 0,
    //         Values = new Dictionary<string, double>(vector.Vector
    //             .Select(x => new KeyValuePair<string, double>(x.Key.Name, x.Value)).ToArray())
    //     };
    //
    //     Assert.Equal(expected.Ki, actual.Ki);
    //     Assert.Equal(expected.Values, actual.Values);
    // }

    // [Fact]
    // public void Mapper_Strategy_To_String()
    // {
    //     var strategy = new EmptyStrategy {Name = "CTT"};
    //
    //     var actual = _mapper.Map<string>(strategy);
    //
    //     Assert.Equal(strategy.Name, actual);
    // }
    //
    // [Fact]
    // public void Mapper_KV_Strategy_Double_To_KV_String_Double()
    // {
    //     var kvp = new KeyValuePair<IStrategy, double>(new EmptyStrategy {Name = "CTT"}, 1);
    //
    //     var actual = _mapper.Map<KeyValuePair<string, double>>(kvp);
    //
    //     Assert.Equal(kvp.Value, actual.Value);
    //     Assert.Equal(kvp.Key.Name, actual.Key);
    // }

    [Fact]
    public void Mapper_MapsIEnumerables()
    {
        // Given

        // When

        // Then
    }

    [Fact]
    public void Mapper_TreeResult()
    {
        // Given
        var researcher = new PopulationFrequency(new SimpleGameRunner());
        var container = new StrategiesContainer();
        // When
        var results = researcher.Research(10, 5, new[] {"C", "D"}.Select(s => container[s]).ToArray(), new[]
        {
            new double[] {4, 0}, new double[] {6, 1}
        });
        // Then
        var result = _mapper.Map<ResultTree>(results.Tree);

        var a = 2;
    }
}