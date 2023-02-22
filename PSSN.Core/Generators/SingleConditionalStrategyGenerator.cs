using PSSN.Core.Containers;
using PSSN.Core.Strategies;
using MathNet.Numerics;

namespace PSSN.Core.Generators;

public class SingleConditionalStrategyGenerator
{
    private readonly int _patternCount;
    private readonly int _parametersCount;
    private readonly int _maxRandomNumber;
    private readonly Random _random;
    private readonly PatternsContainer _container;

    public SingleConditionalStrategyGenerator(int patternCount, int parametersCount, int maxRandomNumber, Random random, PatternsContainer container)
    {
        this._patternCount = patternCount;
        this._parametersCount = parametersCount;
        this._maxRandomNumber = maxRandomNumber;
        this._random = random;
        this._container = container;
    }

    public ConditionalStrategy Generate(string name)
    {
        var coeffs = Enumerable.Range(0, _patternCount).Select(x => Enumerable.Range(0, _parametersCount).Select(x => _random.Next(_maxRandomNumber)).ToArray()).ToList();

        var selected = _container.PattenrsConstructor
            .SelectCombination(_patternCount, _random)
            .Zip(coeffs)
            .Select(kvp => kvp.First.Value(kvp.Second))
            .ToList();
        return new ConditionalStrategy(selected, name);
    }
}