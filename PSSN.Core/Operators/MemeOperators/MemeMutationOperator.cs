using PSSN.Core.Strategies;
using PSSN.Core.Strategies.BehabiourPatterns;

namespace PSSN.Core.Operators.MemeOperators;

public class MemeMutationOperator
{
    private readonly double _chance;
    private readonly Random _random;

    public MemeMutationOperator(double chance, Random random)
    {
        _chance = chance;
        _random = random;
    }

    public ConditionalStrategy Operate(ConditionalStrategy strategy)
    {
        var res = new ConditionalStrategy(){
            Patterns = strategy.Patterns.Select(x => x.Copy()).ToList(),
            Behaviours = strategy.Behaviours,
            Name = strategy.Name
        };
        foreach (var pattern in res.Patterns)
        {
            if (pattern.Coeffs != null)
            {
                var pres = pattern.MutationPresentation;
                for (int i = 0; i < pres.Indexies.Length; ++i)
                {
                    if (_random.Proc(_chance))
                    {
                        pres.Source[pres.Indexies[i].Value] = pres.Source[pres.Indexies[i].Value].Value == 0 ? new IntWrapper(1) : new IntWrapper(0); 
                    }
                }
            }
        }
        return res;
    }

    public IEnumerable<ConditionalStrategy> Operate(IEnumerable<ConditionalStrategy> strategies)
    {
        return strategies.Select(Operate);
    }
}