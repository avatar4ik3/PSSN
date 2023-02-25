using PSSN.Core.Round;
using PSSN.Core.Strategies;
using PSSN.Core.Strategies.BehabiourPatterns;
using FluentAssertions;
namespace PSSN.Core.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var runner = new SimpleGameRunner();

        var strats = new List<ConditionalStrategy>(){
            new ConditionalStrategy(){
                Name = "CTT",
                Patterns = new(){
                    new CttPattern(new int[]{0,0,0,0,0})
                }
            },
            new ConditionalStrategy(){
                Name = "CD",
                Patterns = new(){
                    new CttPattern(new int[]{1,0,1,1,0})
                }
            },
        };

        var tree = runner.Play(strats, new double[][] { new double[] { 4, 0 }, new double[] { 6, 1 } }, 6);
        tree[strats[0], strats[1]].Values.Sum().Should().Be(8);
    }
}