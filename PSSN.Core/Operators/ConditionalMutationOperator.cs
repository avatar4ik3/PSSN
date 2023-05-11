using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class ConditionalMutationOperator
{
    private readonly Random _random;
    private readonly double _chance;
    private readonly int _max;
    private readonly int _min;

    public ConditionalMutationOperator(Random random, double chance, int max, int min)
    {
        this._random = random;
        this._chance = chance;
        this._max = max;
        this._min = min;
    }

    public ConditionalStrategy Operate(ConditionalStrategy strategy)
    {
        if (strategy.Pattern.Coeffs != null)
        {
            for (int i = 0; i < strategy.Pattern.Coeffs.Length; ++i)
            {
                if (_random.Proc(_chance))
                {
                    strategy.Pattern.Coeffs[i].Value = _random.Next(_min, _max);
                }
            }
        }
        return new ConditionalStrategy() { Pattern = strategy.Pattern, Behaviours = strategy.Behaviours, Name = strategy.Name };
    }

    public IEnumerable<ConditionalStrategy> Operate(IEnumerable<ConditionalStrategy> strategies)
    {
        return strategies.Select(Operate);
    }
}