using AutoMapper;
using PSSN.Api.Model;
using PSSN.Api.Profiles;
using PSSN.Common;

namespace PSSN.ApiTests;


public class VectorMapperTets
{
    private IMapper _mapper;
    public VectorMapperTets()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<VectorProfile>());
        _mapper = new Mapper(mapperConfig);
    }

    [Fact]
    public void Mapper_MapsDictionary()
    {
        var vector = new ResultVector
        {
            Stage = 0,
            Vector = new Dictionary<Strategy, double>
            {
                {new Strategy(){Name = "C"},0.125},
                {new Strategy(){Name = "CTT"},0.125},
                {new Strategy(){Name = "CRTT"},0.125},
                {new Strategy(){Name = "CD"},0.125},
                {new Strategy(){Name = "DC"},0.125},
                {new Strategy(){Name = "DTT"},0.125},
                {new Strategy(){Name = "DRTT"},0.125},
                {new Strategy(){Name = "DD"},0.125}
            }
        };

        var actual = _mapper.Map<VectorResponse>(vector);


        var expected = new VectorResponse{
            Ki = 0,
            Values = new Dictionary<string, double>(vector.Vector.Select(x => new KeyValuePair<string,double>(x.Key.Name,x.Value)).ToArray())
        };

        Assert.Equal(expected.Ki,actual.Ki);
        Assert.Equal(expected.Values,actual.Values);

    }

    [Fact]
    public void Mapper_Strategy_To_String(){
        var strategy = new Strategy(){Name = "CTT"};

        var actual = _mapper.Map<string>(strategy);

        Assert.Equal(strategy.Name,actual);
    }

    [Fact]
    public void Mapper_KV_Strategy_Double_To_KV_String_Double(){
        var kvp = new KeyValuePair<Strategy,double>(new Strategy(){Name = "CTT"},1);

        var actual = _mapper.Map<KeyValuePair<string,double>>(kvp);

        Assert.Equal(kvp.Value,actual.Value);
        Assert.Equal(kvp.Key.Name,actual.Key);
    }

    [Fact]
    public void Mapper_MapsIEnumerables()
    {
        // Given
    
        // When
    
        // Then
    }
}