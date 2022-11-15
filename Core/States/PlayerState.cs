using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.States;

public class PlayerState : IPlayerState
{
    public Player p { get; init; }

    public IStrategy strategy { get; set; }
    public List<Behavior> previousBehaviours { get; set; } = new List<Behavior>();

    public Behavior currentBehaviour { get; set; }

    public PlayerState(Player p, IStrategy strategy)
    {
        this.p = p;
        this.strategy = strategy;
        this.currentBehaviour = strategy.StartBehaviour;
    }

    public double Scores { get; set; }
    public bool Next(Game g)
    {
        currentBehaviour = strategy.GetNextBehaviour(g, p);
        return true;
    }
}