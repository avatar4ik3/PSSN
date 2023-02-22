using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class AlwaysD : IStrategy
{
    public string Name { get; set; } = "D";

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        return Behavior.D;
    }
}