using FluentAssertions;
using PSSN.Core.Generators;
using PSSN.Core.Operators;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class SelectionTests
{
    [Fact]
    public void TestName1()
    {
        // Given
        var random = new Random();
        var sg = new SingleFilledStrategyGenerator(6, new Random());
        var generator = new FilledStrategiesGenerator(10, sg);
        var population = generator.Generate();
        var tree = new TreeGameRunnerResult();
        foreach (var s1 in population)
            foreach (var s2 in population)
                foreach (var r in s1.behaviours.Keys)
                {
                    var val = random.NextDouble() * 10;
                    tree[s1, s2, r] = val;
                }

        var op = new SelectionOperator<FilledStrategy>(10, tree, random);
        // When
        var result = op.Operate(population);
        //Then
        var maxValue = tree.map
            .Select(kvp1 => kvp1.Value
                .Select(kvp2 => kvp2.Value.Values
                    .Sum())
                .Sum())
            .Max();
        SelectionOperator<FilledStrategy>.GetTotalScore(result, tree).Should().BeApproximately(maxValue, 1e-5);
    }
}