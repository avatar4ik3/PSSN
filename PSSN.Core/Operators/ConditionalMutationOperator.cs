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
        foreach (var pattern in strategy.Patterns)
        {
            if (pattern.Coeffs != null)
            {
                for (int i = 0; i < pattern.Coeffs.Length; ++i)
                {
                    if (_random.Proc(_chance))
                    {
                        pattern.Coeffs[i] = _random.Next(_min, _max);
                    }
                }
            }
        }
        return new ConditionalStrategy() { Patterns = strategy.Patterns, Behaviours = strategy.Behaviours, Name = strategy.Name };
    }

    public IEnumerable<ConditionalStrategy> Operate(IEnumerable<ConditionalStrategy> strategies)
    {
        return strategies.Select(Operate);
    }
}