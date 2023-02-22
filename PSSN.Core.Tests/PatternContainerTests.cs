using PSSN.Core.Containers;
using PSSN.Core.Generators;
using FluentAssertions;
namespace PSSN.Core.Tests;

public class PatternContainerTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var container = new PatternsContainer();
        // When
        var b = container.CreatePattern("CttPattern", new int[] { 1 });
        // Then
        b.Coeffs.Should().ContainInOrder(1);
    }

    [Fact]
    public void TestName1()
    {
        // Given
        var container = new PatternsContainer();

        var generator = new SingleConditionalStrategyGenerator(1, 2, 6, new Random(), container);
        // When
        var res = generator.Generate("1");
        res.Patterns.Should().HaveCount(1);
        res.Patterns[0].Coeffs.Should().HaveCount(2);
        // Then
    }
}