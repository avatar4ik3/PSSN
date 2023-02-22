using PSSN.Core.Round;
using PSSN.Core.Strategies.BehabiourPatterns;
namespace PSSN.Core.Strategies;

public class ConditionalStrategy : IStrategy
{
    public Dictionary<int, Behavior> Behaviours { get; set; } = new();

    public List<IBehaviourPattern> Patterns { get; set; }

    public ConditionalStrategy(List<IBehaviourPattern> patterns, string name)
    {
        Patterns = patterns;
        Name = name;
    }
    public ConditionalStrategy(List<IBehaviourPattern> patterns, Dictionary<int, Behavior> behaviours, string name)
    {
        Behaviours = behaviours;
        Patterns = patterns;
        Name = name;
    }
    public string Name { get; set; } = "ConditionalStrategy";

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        foreach (var pattern in Patterns)
        {
            pattern.Apply(g, p, this);
        }
        return Behaviours[g.State.currentStage];
    }
}