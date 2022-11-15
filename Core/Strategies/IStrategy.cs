using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public interface IStrategy
{
    public Behavior StartBehaviour { get; init; }
    public Behavior GetNextBehaviour(Game g, Player p);

    public String Name { get; set; }
}