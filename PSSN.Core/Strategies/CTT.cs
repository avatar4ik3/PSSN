using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class CTT : IStrategy
{
    public Behavior StartBehaviour { get; init; } = Behavior.C;
    public string Name { get; set; } = "CTT";

    // public bool Equals(IStrategy? other)
    // {
    //     if (other is CTT o)
    //     {
    //         return this.Name == o.Name;
    //     }
    //     return false;
    // }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        if (g.State.currentStage == 0) return StartBehaviour;
        var otherState = g.State.GetOpponentPlayerState(p)!;
        return otherState.previousBehaviours[g.State.currentStage - 1];
    }
}