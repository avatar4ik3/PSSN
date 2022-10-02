using PSSN.Core.Strategies;

namespace PSSN.Core.States;

public class PlayerState : State
{
    public Player p {get;init;}

    public StrategyBase strategy {get;set;}
    public List<Behavior> previousBehaviours {get;set;} = new List<Behavior>();

    public Behavior currentBehaviour {get;set;}

    public PlayerState(Player p, StrategyBase strategy) {
        this.p = p;
        this.strategy = strategy;
        this.currentBehaviour = strategy.StartBehaviour;
    }

    public int scores {get;set;}
    public bool Next(Game g)
    {
        currentBehaviour = strategy.GetNextBehaviour(g,p);
        return true;
    }
}