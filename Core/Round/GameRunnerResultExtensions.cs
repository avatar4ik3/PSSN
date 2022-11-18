using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public static class GameRunnerResultExtensions
{


    // public static TreeGameRunnerResult ToTree(this IEnumerable<GameRunnerResult> result)
    // {
    //     var tree = new TreeGameRunnerResult();
    //     foreach (var res in result)
    //     {
    //         tree[res.S1, res.S2] = res.Score1;
    //         tree[res.S2, res.S1] = res.Score2;
    //     }
    //     return tree;
    // }

    public static double[,] ToArray(this TreeGameRunnerResult result)
    {
        var array = new double[result.map.Count, result.map.Count];
        List<(IStrategy s1, IStrategy s2, double Total)> shrimped = new();
        foreach (var kvp1 in result.map)
        {
            foreach (var kvp2 in result.map[kvp1.Key])
            {
                shrimped.Add(new(kvp1.Key, kvp2.Key, result[kvp1.Key, kvp2.Key].Values.Sum()));
            }
        }
        var strats = result.map.Keys;
        var indxs = strats.Zip(Enumerable.Range(0, strats.Count)).ToDictionary(i => i.First, s => s.Second);
        foreach (var record in shrimped)
        {
            array[indxs[record.s1], indxs[record.s2]] = record.Total;
        }
        return array;
    }

    public static double[,] ToArray(this IEnumerable<GameRunnerResult> result)
    {
        var strategies = result.Select(r => r.S1).ToHashSet();
        var array = new double[strategies.Count, strategies.Count];
        var dic = strategies.Zip(Enumerable.Range(0, strategies.Count)).ToDictionary(i => i.First, s => s.Second);
        foreach (var r in result)
        {
            array[dic[r.S1], dic[r.S2]] = r.Score1;
            array[dic[r.S2], dic[r.S1]] = r.Score2;
        }
        return array;
    }
}
