using FluentAssertions;
using PSSN.Core;
using PSSN.Core.Operators;
using PSSN.Core.Strategies;

namespace PSSN.CoreTests;

public class MutationsTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var random = new Random();
        var dicLength = 10;
        var g = new SingleFilledStrategyGenerator(dicLength, random);
        var prev = g.Generate();
        var mutator = new MutationOperator(1, random);
        // When
        var actual = (FilledStrategy)mutator.Operate(prev);
        // Then
        actual.behaviours.Should().AllSatisfy((kvp) =>
            kvp.Value.Should().Be(prev.behaviours[kvp.Key].Other())
        );
    }
}