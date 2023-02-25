using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class DefaultCrossingOverOperator
{
    private readonly int _crossingSelector;

    public DefaultCrossingOverOperator(int crossingSelector)
    {
        _crossingSelector = crossingSelector;
    }

    public IEnumerable<ConditionalStrategy> Operate(ConditionalStrategy s1, ConditionalStrategy s2)
    {
        var len = s1.Behaviours.Count;
        var dic1 = new Dictionary<int, Behavior>();
        var dic2 = new Dictionary<int, Behavior>();
        foreach (var i in ..(len - 1))
        {
            dic1[i] = s1.Behaviours[i];
            dic2[i] = s2.Behaviours[i];
        }

        foreach (var i in (len - 1)..(len - _crossingSelector))
        {
            dic1[i] = s2.Behaviours[i];
            dic2[i] = s1.Behaviours[i];
        }

        var ns1 = new ConditionalStrategy() { Patterns = s1.Patterns, Behaviours = dic1, Name = s1.Name };
        var ns2 = new ConditionalStrategy() { Patterns = s2.Patterns, Behaviours = dic2, Name = s2.Name };

        return new[] { ns1, ns2 };
    }
}