using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class MutationOperator
{
    private readonly double _chance;
    private readonly Random _random;

    public MutationOperator(double chance, Random random)
    {
        _chance = chance;
        _random = random;
    }

    public FilledStrategy Operate(FilledStrategy strategy)
    {
        var behs = new Dictionary<int, Behavior>();

        foreach (var key in strategy.behaviours.Keys)
            if (_random.Proc(_chance))
                behs[key] = strategy.behaviours[key].Other();
            else
                behs[key] = strategy.behaviours[key];
        return new FilledStrategy(behs, strategy.Name);
    }

    public IEnumerable<FilledStrategy> Operate(IEnumerable<FilledStrategy> strategies)
    {
        return strategies.Select(Operate);
    }
}