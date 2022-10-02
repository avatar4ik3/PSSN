using MathNet.Numerics.LinearAlgebra;
using PSSN.Core.Strategies;
using MathNet.Numerics.LinearAlgebra.Double;
namespace PSSN.Core.Matrices;

public class PopulationFrequency
{
    public List<Vector<double>> listP = new List<Vector<double>>();
    public List<Vector<double>> Research(int cycleCount, params IStrategy[] strategies){
        
        var vectorElls = Vector<double>.Build.Dense(strategies.Length,(i) => 1 / strategies.Length);
        listP.Add(vectorElls);

        var A = Matrix<double>.Build.DenseOfArray(MatrixPayoff.buildInStageMatrix(strategies));

        for (int cycleIndexer = 1; cycleIndexer <= cycleCount;++cycleIndexer) {
            
            var PPrev = listP[cycleIndexer - 1];
            var matrixProd = A * PPrev;
            var matrixMult = matrixProd.DotProduct(PPrev);

            var PElls = Vector<double>.Build.Dense(strategies.Length, i => {
                return (PPrev[i] * matrixProd[i]) / matrixMult;
            });

            listP.Add(PElls);
        }
        return listP;
    }
}