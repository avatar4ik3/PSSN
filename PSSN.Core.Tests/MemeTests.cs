using PSSN.Core.Operators.MemeOperators;
using PSSN.Core.Strategies;
using PSSN.Core.Strategies.BehabiourPatterns;
using Moq;
namespace PSSN.Core.Tests;

public class MemeTests
{
    [Fact]
    public void CTT_Conversion()
    {
        // Given
        var c = new int[]{0,1,0,0,1};
        var cttBeh = new CttPattern(c);
        // When
        var a = cttBeh.CrossingOverPresentation;
        var b = cttBeh.MutationPresentation;
        // cttBeh.I1 = 1231231123;
        a.Indexies[1].Value = 1231231123;
        // b.Value[0] = 0;
        // Then
        var d = 1;
    }

    [Fact]
    public void Meme_Conversion()
    {
        // Given
        var c = new int[]{0,1,0,1,1,1};
        var meme = new MemePattern(c);
        // When
        var a = meme.CrossingOverPresentation;
        var b = meme.MutationPresentation;
        a.Indexies[1].Value = 1312312;
        // Then
        var d = 1;
    }

    [Fact]
    public void Meme_mutation()
    {
        // Given
        var c = new int[]{0,1,0,1,1,1};
        var meme = new MemePattern(c);
        var strat = new ConditionalStrategy(){
            Patterns = new List<IBehaviourPattern>(){
                meme
            },
            Name = ":1",
        };
        var mutationOperator = new MemeMutationOperator(1,new Random());
        // When
        var mutated = mutationOperator.Operate(strat);
        // Then
        var a = 2;
    }

    [Fact]
    public void Meme_crossingover()
    {
        // Given
        var s1 = new ConditionalStrategy(){
            Patterns = new(){
                new MemePattern(new int[]{3,1,1,1,1,1})
            },
            Name = "1"
        };
        var s2 = new ConditionalStrategy(){
            Patterns = new(){
                new CttPattern(new int[]{0,0,0,0,0})
            },
            Name = "2"
        };

        var crossingOverOperator = new MemeCrossingOverOperator(null,null);
        // When
        var res = crossingOverOperator.Operate(s1,s2);
        // Then
    }
}