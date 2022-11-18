using System.Collections;
using System.Diagnostics.CodeAnalysis;
using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public class TreeGameRunnerResult : IEnumerable<KeyValuePair<IStrategy, Dictionary<IStrategy, Dictionary<int, double>>>>
{
    internal readonly Dictionary<IStrategy, Dictionary<IStrategy, Dictionary<int, double>>> map = new(new StrategyComparer());

    public Dictionary<int, double> this[IStrategy s1, IStrategy s2]
    {
        get => map[s1][s2];
        set
        {
            if (map.ContainsKey(s1) is false)
            {
                map[s1] = new();
            }
            map[s1][s2] = value;
        }
    }

    public double this[IStrategy s1, IStrategy s2, int round]
    {
        get => map[s1][s2][round];
        set
        {
            if (map.ContainsKey(s1) is false)
            {
                map[s1] = new();
            }
            if (map[s1].ContainsKey(s2) is false)
            {
                map[s1][s2] = new();
            }
            map[s1][s2][round] = value;
        }
    }

    public Dictionary<IStrategy, Dictionary<int, double>> this[IStrategy s]
    {
        get => map[s];
    }

    public IEnumerator<KeyValuePair<IStrategy, Dictionary<IStrategy, Dictionary<int, double>>>> GetEnumerator()
    {
        return map.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return map.GetEnumerator();
    }
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