using PSSN.Core.States;
using PSSN.Core.Strategies;
using MathNet.Numerics;
using PSSN.Core.Round;

namespace PSSN.Core.Operators;

public class SelectionOperator
{
    private readonly int _k;
    private readonly Random _random;
    private readonly TreeGameRunnerResult _result;

    public SelectionOperator(int k, Random random, TreeGameRunnerResult result)
    {
        this._k = k;
        this._random = random;
        this._result = result;
    }

    public IStrategy Operate(IEnumerable<IStrategy> strategies)
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