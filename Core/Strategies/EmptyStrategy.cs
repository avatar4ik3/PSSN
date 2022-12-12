using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public class EmptyStrategy : IStrategy
{
    public Behavior StartBehaviour { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public string Name { get; set; }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        throw new NotImplementedException();
    }

    public EmptyStrategy(string name)
    {
        Name = name;
    }

    public EmptyStrategy()
    {

    }
}
