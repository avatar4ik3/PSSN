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

    public ConditionalStrategy Operate(ConditionalStrategy strategy)
    {
        var behs = new Dictionary<int, Behavior>();

        foreach (var key in strategy.Behaviours.Keys)
            if (_random.Proc(_chance))
                behs[key] = strategy.Behaviours[key].Other();
            else
                behs[key] = strategy.Behaviours[key];
        return new ConditionalStrategy() { Patterns = strategy.Patterns, Behaviours = behs, Name = strategy.Name };
    }

    public IEnumerable<ConditionalStrategy> Operate(IEnumerable<ConditionalStrategy> strategies)
    {
        return strategies.Select(Operate);
    }
}