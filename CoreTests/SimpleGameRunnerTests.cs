using PSSN.Core.Matrices;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.CoreTests;

public class SimpleGameRunnerTests
{
    [Fact]
    public void NewGamesRunner_PerfomesAsOld()
    {
        // Given
        var runner = new SimpleGameRunner();
        var oldRunner = new MatrixPayoff(MatrixPayoff.payoffs);
        var strategies = new IStrategy[]{
                new AlwaysC(),
                new AlwaysD()
            };
        // When
        var results = runner.Play(
            strategies,
            new double[][]{
                new double []{4,0},new double[]{6,1}
            }
        );

        var expected = MatrixPayoff.buildInStageMatrix(strategies);
        var actual = results;
        // Then
        var c = new AlwaysC();
        var d = new AlwaysD();

        Assert.Equal(actual[c, c].Values.Sum(), expected[0, 0]);
        Assert.Equal(actual[c, d].Values.Sum(), expected[0, 1]);
        Assert.Equal(actual[d, c].Values.Sum(), expected[1, 0]);
        Assert.Equal(actual[d, d].Values.Sum(), expected[1, 1]);

    }
}