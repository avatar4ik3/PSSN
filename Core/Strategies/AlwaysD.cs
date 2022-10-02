namespace PSSN.Core.Strategies;

public sealed class AlwaysD : StrategyBase
{
    public override Behavior StartBehaviour { get ; init; } = Behavior.D;
    public override string Name { get; init; } = "D";

    public override Behavior GetNextBehaviour(Game g, Player p)
    {
        return StartBehaviour;
    }
}