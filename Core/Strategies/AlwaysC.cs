using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class AlwaysC : StrategyBase
{
    public override Behavior StartBehaviour { get ; init; } = Behavior.C;
    public override string Name { get; init; } = "C";

    public override Behavior GetNextBehaviour(Game g, Player p)
    {
        return StartBehaviour;
    }
}