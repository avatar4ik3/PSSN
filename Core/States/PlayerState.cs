using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.States;

public class PlayerState : IPlayerState
{
    public Player p { get; init; }

    public IStrategy strategy { get; set; }
    public List<Behavior> previousBehaviours { get; set; } = new List<Behavior>();

    public Behavior currentBehaviour { get; set; }

    public Dictionary<int, double> Scores { get; set; } = new Dictionary<int, double>();

    public double TotalScore { get => Scores.Values.Sum(); set => _ = value; }

    public PlayerState(Player p, IStrategy strategy)
    {
        this.p = p;
        this.strategy = strategy;
        this.currentBehaviour = strategy.StartBehaviour;
    }


    public bool Next(Game g)
    {
        currentBehaviour = strategy.GetNextBehaviour(g, p);
        return true;
    }
}