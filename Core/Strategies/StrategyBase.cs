using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public abstract class StrategyBase : IStrategy
{
    public abstract Behavior StartBehaviour { get; init; }
    public abstract string Name { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is StrategyBase @base &&
               StartBehaviour == @base.StartBehaviour &&
               Name == @base.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartBehaviour, Name);
    }

    public abstract Behavior GetNextBehaviour(Game g, Player p);


}