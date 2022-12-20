using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public abstract class FixedCTTStrategyBase : StrategyBase
{
    private readonly IStrategy _ctt = new CTT();

    protected FixedCTTStrategyBase(int delimeter, string name)
    {
        _delimeter = delimeter;
        Name = name;
    }

    private int _delimeter { get; }
    public override Behavior StartBehaviour { get; init; } = Behavior.C;
    public override string Name { get; set; }

    public override Behavior GetNextBehaviour(Game g, Player p)
    {
        if (g.State.currentStage < _delimeter) return _ctt.GetNextBehaviour(g, p);
        return Behavior.D;
    }
}