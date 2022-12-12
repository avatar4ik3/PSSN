using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class FilledStrategy : IStrategy
{
    public Dictionary<int, Behavior> behaviours { get; set; }
    public Behavior StartBehaviour
    {
        get => behaviours[0];
        init => throw new NotSupportedException();
    }
    public String Name { get; set; } = "Filled";

    public FilledStrategy(Dictionary<int, Behavior> behaviours)
    {
        this.behaviours = behaviours;
    }

    public FilledStrategy(Dictionary<int, Behavior> behaviours, String name)
    {
        this.Name = name;
        this.behaviours = behaviours;
    }

    public FilledStrategy()
    {
        this.behaviours = new();
    }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        return behaviours[g.State.currentStage];
    }

    // public bool Equals(IStrategy? other)
    // {
    //     if (other is FilledStrategy o)
    //     {
    //         return this.Name.Equals(o.Name);
    //     }
    //     return false;
    // }

    // public override int GetHashCode()
    // {
    //     return HashCode.Combine(behaviours, Name);
    // }
}