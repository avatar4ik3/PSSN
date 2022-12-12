using MathNet.Numerics.LinearAlgebra;
using PSSN.Core.Strategies;
using MathNet.Numerics.LinearAlgebra.Double;
using PSSN.Core.Round;

namespace PSSN.Core.Matrices;

public class PopulationFrequency
{
    private readonly IGameRunner _runner;
    public List<Vector<double>> listP = new List<Vector<double>>();

    public PopulationFrequency(IGameRunner runner)
    {
        this._runner = runner;
    }

    public (List<Vector<double>> Vectors, TreeGameRunnerResult Tree) Research(int cycleCount, int roundCount, IStrategy[] strategies, double[][] payoffs)
    {

        var vectorElls = Vector<double>.Build.Dense(strategies.Length, (i) => 1.0 / strategies.Length);
        listP.Add(vectorElls);

        var tree = _runner.Play(strategies, payoffs, roundCount);

        var A = Matrix<double>.Build.DenseOfArray(tree.ToArray());

        for (int cycleIndexer = 1; cycleIndexer <= cycleCount; ++cycleIndexer)
        {
            var PPrev = listP[cycleIndexer - 1];
            var matrixProd = A * PPrev;
            var matrixMult = matrixProd.DotProduct(PPrev);

            var PElls = Vector<double>.Build.Dense(strategies.Length, i =>
            {
                return (PPrev[i] * matrixProd[i]) / matrixMult;
            });

            listP.Add(PElls);
        }
        return new(listP, tree);
    }
}