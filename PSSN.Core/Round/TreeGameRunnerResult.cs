using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public class TreeGameRunnerResult
{
    public ConcurrentDictionary<IStrategy, ConcurrentDictionary<IStrategy, Dictionary<int, double>>> RawMap = new(new StrategyComparer());

    public Dictionary<int, double> this[IStrategy s1, IStrategy s2]
    {
        get => RawMap[s1][s2];
        set
        {
            if (RawMap.ContainsKey(s1) is false) RawMap[s1] = new ConcurrentDictionary<IStrategy, Dictionary<int, double>>();
            RawMap[s1][s2] = value;
        }
    }

    public double this[IStrategy s1, IStrategy s2, int round]
    {
        get => RawMap[s1][s2][round];
        set
        {
            if (RawMap.ContainsKey(s1) is false) RawMap[s1] = new ConcurrentDictionary<IStrategy, Dictionary<int, double>>();
            if (RawMap[s1].ContainsKey(s2) is false) RawMap[s1][s2] = new Dictionary<int, double>();
            RawMap[s1][s2][round] = value;
        }
    }

    public ConcurrentDictionary<IStrategy, Dictionary<int, double>> this[IStrategy s] => RawMap[s];
}

public class StrategyComparer : IEqualityComparer<IStrategy>
{
    public bool Equals(IStrategy? x, IStrategy? y)
    {
        return x?.Id == y?.Id;
    }

    public int GetHashCode([DisallowNull] IStrategy obj)
    {
        return obj.Id.GetHashCode();
    }
}