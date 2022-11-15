using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class CrossingOverOperator : IOperator<IStrategy, IStrategy>
{
    private readonly int _crossingSelector;

    public CrossingOverOperator(int crossingSelector)
    {
        this._crossingSelector = crossingSelector;
    }
    public IEnumerable<IStrategy> Operate(IEnumerable<IStrategy> strategies)
    {
        if (strategies.Count() != 2 || strategies is not IEnumerable<FilledStrategy>)
        {
            throw new ArgumentException("Strateges must be count of 2 and be FilledStrategy only");
        }
        var strats = strategies.Select(s => s as FilledStrategy).ToArray();
        var s1 = strats[0]!;
        var s2 = strats[1]!;

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

        var ns1 = new FilledStrategy(dic1);
        var ns2 = new FilledStrategy(dic2);

        return new List<FilledStrategy>() { ns1, ns2 };
    }

    public IStrategy Operate(IStrategy strategies)
    {
        throw new NotImplementedException();
    }
}