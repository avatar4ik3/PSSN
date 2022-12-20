using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.States;

public class PlayerState : IPlayerState
{
    public PlayerState(Player p, IStrategy strategy)
    {
        this.p = p;
        this.strategy = strategy;
        currentBehaviour = strategy.StartBehaviour;
    }

    public Player p { get; init; }

    public IStrategy strategy { get; set; }
    public List<Behavior> previousBehaviours { get; set; } = new();

    public Behavior currentBehaviour { get; set; }

    public Dictionary<int, double> Scores { get; set; } = new();

    public double TotalScore
    {
        get => Scores.Values.Sum();
        set => _ = value;
    }

    public bool Next(Game g)
    {
        currentBehaviour = strategy.GetNextBehaviour(g, p);
        return true;
    }
}