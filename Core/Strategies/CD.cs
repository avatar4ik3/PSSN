using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class CD : StrategyBase
{
    public override Behavior StartBehaviour { get; init; } = Behavior.C;
    public override String Name { get; set; } = "CD";

    public override Behavior GetNextBehaviour(Game g, Player p)
    {
        if (g.State.currentStage == 0) return StartBehaviour;
        return Behavior.D;
    }
}