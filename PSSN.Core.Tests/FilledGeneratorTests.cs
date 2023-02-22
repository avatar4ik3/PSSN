using FluentAssertions;
using PSSN.Core.Generators;

namespace PSSN.Core.Tests;

public class FilledGeneratorTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var generator = new SingleFilledStrategyGenerator(10, new Random());

        var multiGenerator = new FilledStrategiesGenerator(20, generator);
        // When
        var data = multiGenerator.Generate();
        // Then
        var result = data.ToList();

        result.Should()
            .HaveCount(20).And
            .AllSatisfy(c => c.behaviours.Should().HaveCount(10));
    }
}