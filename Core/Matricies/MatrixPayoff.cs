using PSSN.Core.Round;
using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.Core.Matrices;

public class MatrixPayoff
{
    // TODO: 6/4/2021 сделать нормальное заполнение матрицы!!!!!!!!!! 
    static public double[,] payoffs = new double[,] { { 4, 0 }, { 6, 1 } };

    public MatrixPayoff(double[,] arr)
    {
        payoffs = arr;
    }

    public static double[,] buildInStageMatrix(params IStrategy[] strategies)
    {
        int outSize = strategies.Length;
        double[,] outArr = new double[outSize, outSize];
        Player p1 = new Player("p1");
        Player p2 = new Player("p2");
        for (int s1Index = 0; s1Index < outSize; s1Index++)
        {
            var strategy1 = strategies[s1Index];
            for (int s2Index = 0; s2Index < outSize; s2Index++)
            {
                var strategy2 = strategies[s2Index];
                var ps1 = new PlayerState(p1, (StrategyBase)strategy1);
                var ps2 = new PlayerState(p2, (StrategyBase)strategy2);
                Game g = new Game(ps1, ps2, new double[][]{
                    new double[]{4,0},new double[]{6,1}
                });
                g.Play();
                outArr[s1Index, s2Index] = g.getP1TotalScore();
            }
        }
        return outArr;
    }

    static public int getIndexPayoffByStrategies(Behavior b1, Behavior b2)
    {
        //TODO 
        return -1;
    }
}