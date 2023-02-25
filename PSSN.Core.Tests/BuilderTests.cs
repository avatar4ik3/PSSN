using PSSN.Core.Generators;
using FluentAssertions;
namespace PSSN.Core.Tests;

public class BuilderTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var builder = new ConditionalStrategyBuilder(new Random());
        // When
        var s1 = builder.WithName("1").Build();
        var s2 = builder.WithName("2").Build();
        // Then

        s1.Name.Should().NotBe(s2.Name);
        s1.Should().NotBe(s2);

    }
}