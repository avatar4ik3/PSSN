using PSSN.Core.Round;
using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.Core.Matricies;

[Obsolete($"{nameof(MatrixPayoff)} is deprecated, use {nameof(IGameRunner)} and pass payoffs as a parameter instead.")]
public class MatrixPayoff
{
    private readonly double[,] _payoffs = { { 4, 0 }, { 6, 1 } };

    public MatrixPayoff(double[,] arr)
    {
        _payoffs = arr;
    }

    [Obsolete($"{nameof(buildInStageMatrix)} is deprecated, use {nameof(IGameRunner)} instead.")]
    public static double[,] buildInStageMatrix(params IStrategy[] strategies)
    {
        var outSize = strategies.Length;
        var outArr = new double[outSize, outSize];
        var p1 = new Player("p1");
        var p2 = new Player("p2");
        for (var s1Index = 0; s1Index < outSize; s1Index++)
        {
            var strategy1 = strategies[s1Index];
            for (var s2Index = 0; s2Index < outSize; s2Index++)
            {
                var strategy2 = strategies[s2Index];
                var ps1 = new PlayerState(p1, strategy1);
                var ps2 = new PlayerState(p2, strategy2);
                var g = new Game(
                    new GameState(ps1, ps2, 6),
                    new[]
                    {
                        new double[] {4, 0},
                        new double[] {6, 1}
                    }
                );
                g.Play();
                outArr[s1Index, s2Index] = g.P1Scores().Values.Sum();
            }
        }

        return outArr;
    }

    public static int getIndexPayoffByStrategies(Behavior b1, Behavior b2)
    {
        return -1;
    }
}