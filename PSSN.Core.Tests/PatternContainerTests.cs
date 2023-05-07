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
        // b.Coeffs.Should().ContainInOrder(1);
    }

    [Fact]
    public void TestName1()
    {
        // Given
        var container = new PatternsContainer();

        var generator = ConditionalStrategyBuilder.Random(new Random(), 1, 1, 2, 6, container);
        // When
        // var res = generator.First();
        // res.Patterns.Should().HaveCount(1);
        // res.Patterns[0].Coeffs.Should().HaveCount(2);
        // Then
    }

    [Fact]
    public void TestName2()
    {
        // Given
        var container = new PatternsContainer();

        var generator = ConditionalStrategyBuilder.RandomMemes(
            new Random(),
            0,
            1,
            5,
            container
        ).ToList();
        // When
        // var res = generator.First();
        // res.Patterns.Should().HaveCount(1);
        // res.Patterns[0].Coeffs.Should().HaveCount(2);
        // Then
    }
}