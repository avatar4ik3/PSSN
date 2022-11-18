using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public interface IGameRunner
{
    public TreeGameRunnerResult Play(IEnumerable<IStrategy> strategies, double[][] payoffs);
}
