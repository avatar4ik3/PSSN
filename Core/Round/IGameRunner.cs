using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public interface IGameRunner
{
    public IEnumerable<GameRunnerResult> Play(IEnumerable<IStrategy> strategies, double[][] payoffs);
}

public record GameRunnerResult(
    IStrategy S1,
    IStrategy S2,
    double Score1,
    double Score2
);

public static class GameRunnerResultExtensions{

    public static TreeGameRunnerResult ToTree(this IEnumerable<GameRunnerResult> result){
        var tree = new TreeGameRunnerResult();
        foreach(var res in result){
            tree[res.S1,res.S2] = res.Score1;
            tree[res.S2,res.S1] = res.Score2;
        }
        return tree;
    }

    public static double[,] ToArray(this IEnumerable<GameRunnerResult> result){
        var strategies = result.Select(r => r.S1).ToHashSet();
        var array = new double[strategies.Count,strategies.Count];
        var dic = strategies.Zip(Enumerable.Range(0,strategies.Count)).ToDictionary(i => i.First,s => s.Second);
        foreach(var r in result){
            array[dic[r.S1],dic[r.S2]] = r.Score1;
            array[dic[r.S2],dic[r.S1]] = r.Score2;
        }
        return array;
    }
}

public class TreeGameRunnerResult
{
    private readonly Dictionary<IStrategy, Dictionary<IStrategy, double>> map = new Dictionary<IStrategy, Dictionary<IStrategy, double>>();

    public double this[IStrategy s1, IStrategy s2]
    {
        get => map[s1][s2];
        set
        {
            if (map.ContainsKey(s1) is false)
            {
                map[s1] = new Dictionary<IStrategy, double>();
            }
            map[s1][s2] = value;
        }
    }
}