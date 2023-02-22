using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public class FilledStrategy : IStrategy
{
    public FilledStrategy(Dictionary<int, Behavior> behaviours)
    {
        this.behaviours = behaviours;
    }

    public FilledStrategy(Dictionary<int, Behavior> behaviours, string name)
    {
        Name = name;
        this.behaviours = behaviours;
    }

    public FilledStrategy()
    {
        behaviours = new Dictionary<int, Behavior>();
    }

    public Dictionary<int, Behavior> behaviours { get; set; }

    public string Name { get; set; } = "Filled";

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        return behaviours[g.State.currentStage];
    }
}