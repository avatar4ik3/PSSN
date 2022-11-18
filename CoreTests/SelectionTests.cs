using Moq;
using PSSN.Core.States;
using PSSN.Core;
using PSSN.Core.Operators;
using FluentAssertions;
using PSSN.Core.Round;
using System;

namespace PSSN.CoreTests;

public class SelectionTests
{
    [Fact]
    public void TestName1()
    {
        // Given
        Random random = new Random();
        var sg = new SingleFilledStrategyGenerator(6, random);
        var generator = new FilledStrategiesGenerator(10, sg);
        var population = generator.Generate();
        var tree = new TreeGameRunnerResult();
        foreach (var s1 in population)
        {
            foreach (var s2 in population)
            {
                foreach (var r in s1.behaviours.Keys)
                {
                    var val = random.NextDouble() * 10;
                    tree[s1, s2, r] = val;
                }
            }
        }

        var op = new SelectionOperator(10, random, tree);
        // When
        var result = op.Operate(population);
        //Then
        var maxValue = tree
            .Select(kvp1 => kvp1.Value
                            .Select(kvp2 => kvp2.Value.Values
                            .Sum())
                    .Sum())
            .Max();
        SelectionOperator.GetTotalScore(result, tree).Should().BeApproximately(maxValue, 1e-5);
    }
}