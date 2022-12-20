using FluentAssertions;

using PSSN.Core.Operators;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class MutationsTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var random = new Random();
        var dicLength = 10;
        var g = new SingleFilledStrategyGenerator(dicLength);
        var prev = g.Generate();
        var mutator = new MutationOperator(1);
        // When
        var actual = (FilledStrategy) mutator.Operate(prev);
        // Then
        actual.behaviours.Should().AllSatisfy(kvp =>
            kvp.Value.Should().Be(prev.behaviours[kvp.Key].Other())
        );
    }
}