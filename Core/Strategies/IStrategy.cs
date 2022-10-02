namespace PSSN.Core.Strategies;

public interface IStrategy 
{
    public Behavior GetNextBehaviour(Game g, Player p);

    public String Name { get; init; }
}