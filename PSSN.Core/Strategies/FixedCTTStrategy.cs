using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public abstract class FixedCTTStrategyBase : IStrategy
{
    private readonly IStrategy _ctt = new CTT();

    protected FixedCTTStrategyBase(int delimeter, string name)
    {
        _delimeter = delimeter;
        Name = name;
    }

    private int _delimeter { get; }
    public string Name { get; set; }

    public Behavior GetNextBehaviour(Game g, Player p)
    {
        if (g.State.currentStage < _delimeter) return _ctt.GetNextBehaviour(g, p);
        return Behavior.D;
    }
}