using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class MutationOperator
{
    private readonly double _chance;
    private static Random? _random;

    public MutationOperator(double chance)
    {
        _chance = chance;
        _random = new Random();
    }

    public IStrategy Operate(FilledStrategy strategy)
    {
        var behs = new Dictionary<int, Behavior>();
        
        foreach (var key in strategy.behaviours.Keys)
            if (_random.Proc(_chance))
                behs[key] = strategy.behaviours[key].Other();
            else
                behs[key] = strategy.behaviours[key];
        return new FilledStrategy(behs, strategy.Name);
    }

    public IEnumerable<IStrategy> Operate(IEnumerable<FilledStrategy> strategies)
    {
        return strategies.Select(Operate);
    }
}