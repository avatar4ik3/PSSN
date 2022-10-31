using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public sealed class FilledStrategy : IStrategy
{
    public Dictionary<int, Behavior> behaviours { get; set; }
    public Behavior StartBehaviour
    {
        get => behaviours[0];
        init => behaviours[0] = value;
    }
    string IStrategy.Name { get; init; } = "Filled";

    public FilledStrategy(Dictionary<int, Behavior> behaviours)
    {
        this.behaviours = behaviours;
    }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        return behaviours[g.State.currentStage];
    }
}