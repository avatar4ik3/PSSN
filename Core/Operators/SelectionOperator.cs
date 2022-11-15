using PSSN.Core.States;
using PSSN.Core.Strategies;
using MathNet.Numerics;
namespace PSSN.Core.Operators;

public class SelectionOperator : IOperator<IPlayerState, IStrategy>
{
    private readonly int _k;
    private readonly Random _random;

    public SelectionOperator(int k, Random random)
    {
        this._k = k;
        this._random = random;
    }

    public IEnumerable<IStrategy> Operate(IEnumerable<IPlayerState> strategies)
    {
        var selected = strategies.SelectCombination(_k, _random).ToList();
        var scores = selected.Select(s => s.Scores);
        return new List<IStrategy>() {
            selected.MaxBy(s => s.Scores)?.strategy
            ?? throw new ArgumentException($"Provided strategies must contain at least 1 chunk with size [0,${_k}]")
        };
    }

    public IStrategy Operate(IPlayerState strategies)
    {
        throw new NotImplementedException("SelectionOperator does not support single value opeartions");
    }
}