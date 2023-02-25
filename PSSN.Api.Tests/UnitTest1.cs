using AutoMapper;
using PSSN.Api.Profiles;
using PSSN.Common.Model;
using PSSN.Core.Containers;
using PSSN.Core.Strategies;

namespace PSSN.Api.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var mapper = new Mapper(new MapperConfiguration(opt => opt.AddProfile<VectorProfile>()));
        var container = new PatternsContainer();
        var res = mapper.Map<ConditionalStrategyModel, ConditionalStrategy>(new ConditionalStrategyModel()
        {
            Name = "1",
            Patterns = new List<PatternModel>{
                new PatternModel(){
                    Name = "CttPattern",
                    Coeffs = new int[]{1,2,3,4,5,6,7,8,10}
                }
            }
        }, opt => opt.AfterMap((src, dest) => dest.Patterns = src.Patterns.Select(x => container.CreatePattern(x.Name, x.Coeffs)).ToList()));
    }
}