using PSSN.Core.Containers;
using PSSN.Core.Matricies;
using PSSN.Core.Round;

namespace PSSN.Core.Tests;

public class ResearcherTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var researcher = new PopulationFrequency(new SimpleGameRunner());
        var container = new StrategiesContainer();
        // When
        var results = researcher.Research(10, 5, new[] { "C", "D" }.Select(s => container[s]).ToArray(), new[]
        {
            new double[] {4, 0}, new double[] {6, 1}
        });
        // Then
        var assertResultsManially = 0;
    }
}