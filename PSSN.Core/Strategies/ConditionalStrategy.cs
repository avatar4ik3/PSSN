using PSSN.Core.Round;
using PSSN.Core.Strategies.BehabiourPatterns;
namespace PSSN.Core.Strategies;

public class ConditionalStrategy : IStrategy
{
    public Dictionary<int, Behavior> Behaviours { get; set; } = new();

    public IBehaviourPattern Pattern { get; set; }

    public string Name { get; set; } = "ConditionalStrategy";
    public int Id { get; set; }

    public ConditionalStrategy()
    {

    }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        Pattern.Apply(g, p, this);
        return Behaviours[g.State.currentStage];
    }
}