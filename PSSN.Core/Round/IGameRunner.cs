using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public interface IGameRunner : IDisposable
{
    public TreeGameRunnerResult Play(IEnumerable<IStrategy> strategies, double[][] payoffs, int rounds);
}