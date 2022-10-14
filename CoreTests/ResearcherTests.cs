using PSSN.Core.Matrices;
using PSSN.Core;
using PSSN.Core.Round;

namespace PSSN.CoreTests;

public class ResearcherTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var researcher = new PopulationFrequency(new SimpleGameRunner());
        var container = new StrategesContainer();
        // When
        var results = researcher.Research(10,new[]{"C","D"}.Select(s => container[s]).ToArray(),new double[][]{
                new double []{4,0},new double[]{6,1}
            });
        // Then
        var assertResultsManially = 0;
    }
}