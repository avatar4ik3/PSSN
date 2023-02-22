using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class AlwaysC : IStrategy
{
    public string Name { get; set; } = "C";

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        return Behavior.C;
    }
}