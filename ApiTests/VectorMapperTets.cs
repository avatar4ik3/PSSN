using AutoMapper;
using PSSN.Api.Model;
using PSSN.Api.Profiles;
using PSSN.Common;

namespace PSSN.ApiTests;

[TestClass]
public class VectorMapperTets
{
    private IMapper _mapper;
    [TestInitialize()]
    public void init()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<VectorProfile>());
        _mapper = new Mapper(mapperConfig);
    }

    [TestMethod]
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

        Assert.AreEqual(expected.Ki,actual.Ki);
        CollectionAssert.AreEquivalent(expected.Values,actual.Values);

    }

    [TestMethod]
    public void Mapper_Strategy_To_String(){
        var strategy = new Strategy(){Name = "CTT"};

        var actual = _mapper.Map<string>(strategy);

        Assert.AreEqual(strategy.Name,actual);
    }

    [TestMethod]
    public void Mapper_KV_Strategy_Double_To_KV_String_Double(){
        var kvp = new KeyValuePair<Strategy,double>(new Strategy(){Name = "CTT"},1);

        var actual = _mapper.Map<KeyValuePair<string,double>>(kvp);

        Assert.AreEqual(kvp.Value,actual.Value);
        Assert.AreEqual(kvp.Key.Name,actual.Key);
    }
}