using FluentAssertions;
using PSSN.Core.Operators.MemeOperators;
using PSSN.Core.Strategies;
using PSSN.Core.Strategies.BehabiourPatterns;
using Moq;
using PSSN.Core.Containers;
using PSSN.Core.Generators;
using PSSN.Core.Round;
namespace PSSN.Core.Tests;

public class MemeTests
{
    [Fact]
    public void CTT_vs_CTT_must_play_only_C()
    {
        var payoffs = new double[][] { new double[] { 4, 0 }, new double[] { 6, 1 } };
        var _patternsContainer = new PatternsContainer();
        var gameLength = 6;
        var strats = ConditionalStrategyBuilder.RandomMemes(new Random(), 1, 2, gameLength, _patternsContainer).ToList();
        var gameRunner = new SimpleGameRunner();

        var results = gameRunner.Play(strats, payoffs, gameLength);

        var isOk = results[strats[0], strats[1]].Values.All(x => Math.Abs(x - 4) < 1e-6);

        Assert.True(isOk);
    }

    [Fact]
    public void CTT_Conversion()
    {
        // Given
        var c = new int[] { 0, 1, 0, 0, 1 };
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
        var c = new int[] { 0, 1, 0, 1, 1, 1 };
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
        var c = new int[] { 0, 1, 0, 1, 1, 1 };
        var meme = new MemePattern(c);
        var strat = new ConditionalStrategy()
        {
            Pattern =
                meme
            ,
            Name = ":1",
        };
        var mutationOperator = new MemeMutationOperator(1, new Random());
        // When
        var mutated = mutationOperator.Operate(strat);
        // Then
        var a = 2;
    }

    [Fact]
    public void Meme_crossingover()
    {
        // Given
        var s1 = new ConditionalStrategy()
        {
            Pattern =
                new MemePattern(new int[] { 3, 1, 1, 1, 1, 1 })
      ,
            Name = "1"
        };
        var s2 = new ConditionalStrategy()
        {
            Pattern =
                new CttPattern(new int[] { 0, 0, 0, 0, 0 })
            ,
            Name = "2"
        };

        var crossingOverOperator = new MemeCrossingOverOperator(null, null);
        // When
        var res = crossingOverOperator.Operate(s1, s2);
        // Then
    }
}