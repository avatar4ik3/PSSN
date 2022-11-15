using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class FilledStrategy : IStrategy
{
    public Dictionary<int, Behavior> behaviours { get; set; } = null;
    public Behavior StartBehaviour
    {
        get => behaviours[0];
        init => behaviours[0] = value;
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

    }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        return behaviours[g.State.currentStage];
    }
}