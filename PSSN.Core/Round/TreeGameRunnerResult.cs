using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public class TreeGameRunnerResult
{
    public Dictionary<IStrategy, Dictionary<IStrategy, Dictionary<int, double>>> map = new(new StrategyComparer());

    public Dictionary<int, double> this[IStrategy s1, IStrategy s2]
    {
        get => map[s1][s2];
        set
        {
            if (map.ContainsKey(s1) is false) map[s1] = new Dictionary<IStrategy, Dictionary<int, double>>();
            map[s1][s2] = value;
        }
    }

    public double this[IStrategy s1, IStrategy s2, int round]
    {
        get => map[s1][s2][round];
        set
        {
            if (map.ContainsKey(s1) is false) map[s1] = new Dictionary<IStrategy, Dictionary<int, double>>();
            if (map[s1].ContainsKey(s2) is false) map[s1][s2] = new Dictionary<int, double>();
            map[s1][s2][round] = value;
        }
    }

    public Dictionary<IStrategy, Dictionary<int, double>> this[IStrategy s] => map[s];
}

public class StrategyComparer : IEqualityComparer<IStrategy>
{
    public bool Equals(IStrategy? x, IStrategy? y)
    {
        return x?.Name == y?.Name;
    }

    public int GetHashCode([DisallowNull] IStrategy obj)
    {
        return obj.Name.GetHashCode();
    }
}