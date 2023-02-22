using MathNet.Numerics;

using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class SelectionOperator<T> where T : IStrategy
{
    private readonly Random _random;
    private readonly int _k;
    private readonly TreeGameRunnerResult _result;

    public SelectionOperator(int k, TreeGameRunnerResult result, Random random)
    {
        _k = k;
        _random = random;
        _result = result;
    }

    public T Operate(IEnumerable<T> strategies)
    {
        var selected = strategies.SelectCombination(_k, _random).ToList();
        return selected.MaxBy(s => GetTotalScore(s, _result))
               ?? throw new ArgumentException($"Provided strategies must contain at least 1 chunk with size [0,${_k}]");
    }

    public static double GetTotalScore(IStrategy s, TreeGameRunnerResult tree)
    {
        return tree[s].Select(s2 => tree[s, s2.Key].Values.Sum()).Sum();
    }
}