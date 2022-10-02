using PSSN.Core.Matrices;
using PSSN.Core;
namespace PSSN.CoreTests;

public class ResearcherTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var researcher = new PopulationFrequency();
        var container = new StrategesContainer();
        // When
        var results = researcher.Research(10,new[]{"C","D"}.Select(s => container[s]).ToArray());
        // Then
        var assertResultsManially = 0;
    }
}