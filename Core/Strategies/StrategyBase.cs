namespace PSSN.Core.Strategies;

public abstract class StrategyBase : IStrategy
{
    public abstract Behavior StartBehaviour { get ; init ; }
    public abstract string Name { get; init; }

    public abstract Behavior GetNextBehaviour(Game g, Player p);
}