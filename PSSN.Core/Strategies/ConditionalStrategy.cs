using PSSN.Core.Round;
using PSSN.Core.Strategies.BehabiourPatterns;
namespace PSSN.Core.Strategies;

public class ConditionalStrategy : IStrategy
{
    public Dictionary<int, Behavior> Behaviours { get; set; } = new();

    public List<IBehaviourPattern> Patterns { get; set; } = new();

    public string Name { get; set; } = "ConditionalStrategy";

    public ConditionalStrategy()
    {

    }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        foreach (var pattern in Patterns)
        {
            pattern.Apply(g, p, this);
        }
        return Behaviours[g.State.currentStage];
    }
}