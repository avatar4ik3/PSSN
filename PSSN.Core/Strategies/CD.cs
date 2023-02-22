using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class CD : IStrategy
{
    public string Name { get; set; } = "CD";

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        if (g.State.currentStage == 0) return Behavior.C;
        return Behavior.D;
    }
}