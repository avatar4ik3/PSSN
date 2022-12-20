using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class DefaultCrossingOverOperator
{
    private readonly int _crossingSelector;

    public DefaultCrossingOverOperator(int crossingSelector)
    {
        _crossingSelector = crossingSelector;
    }

    public IEnumerable<IStrategy> Operate(FilledStrategy s1, FilledStrategy s2)
    {
        var len = s1.behaviours.Count;
        var dic1 = new Dictionary<int, Behavior>();
        var dic2 = new Dictionary<int, Behavior>();
        foreach (var i in ..(len - 1))
        {
            dic1[i] = s1.behaviours[i];
            dic2[i] = s2.behaviours[i];
        }

        foreach (var i in (len - 1)..(len - _crossingSelector))
        {
            dic1[i] = s2.behaviours[i];
            dic2[i] = s1.behaviours[i];
        }

        var ns1 = new FilledStrategy(dic1, s1.Name);
        var ns2 = new FilledStrategy(dic2, s2.Name);

        return new List<FilledStrategy> {ns1, ns2};
    }
}