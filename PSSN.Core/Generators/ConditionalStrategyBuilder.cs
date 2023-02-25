using MathNet.Numerics;
using PSSN.Core.Containers;
using PSSN.Core.Strategies;
using PSSN.Core.Strategies.BehabiourPatterns;

namespace PSSN.Core.Generators;

public class ConditionalStrategyBuilder
{
    private ConditionalStrategy _strategy;
    private readonly Random _random;

    public ConditionalStrategyBuilder(Random random)
    {
        this._random = random;
        _strategy = new();
    }
    public static IEnumerable<ConditionalStrategy> Random(Random random, int count, int patternsCount, int parametersCount, int maxRandomNumber, PatternsContainer container)
    {
        for (int i = 0; i < count; ++i)
        {
            var coeffs = Enumerable
                    .Range(0, patternsCount)
                    .Select(x =>
                                Enumerable
                                    .Range(0, parametersCount)
                                    .Select(x =>
                                                random.Next(maxRandomNumber)).ToArray()).ToList();

            var selected = container.PattenrsConstructor
                .SelectCombination(patternsCount, random)
                .Zip(coeffs)
                .Select(kvp => kvp.First.Value(kvp.Second))
                .ToList();
            yield return new ConditionalStrategy()
            {
                Patterns = selected,
                Name = i.ToString()
            };
        }
    }

    public ConditionalStrategyBuilder WithPattern(IBehaviourPattern pattern)
    {
        _strategy.Patterns.Add(pattern);
        return this;
    }

    public ConditionalStrategyBuilder WithName(string name)
    {
        _strategy.Name = name;
        return this;
    }

    public ConditionalStrategyBuilder WithBehaviours(Dictionary<int, Behavior> behaviours)
    {
        _strategy.Behaviours = behaviours;
        return this;
    }

    public ConditionalStrategyBuilder WithRandomBehaviours(int genCount, double disitributionChance)
    {
        foreach (var i in ..genCount)
        {
            _strategy.Behaviours[i] = _random.Proc(disitributionChance) ? Behavior.C : Behavior.D;
        }
        return this;
    }

    public ConditionalStrategy Build()
    {
        var res = _strategy;
        _strategy = new ConditionalStrategy();
        return res;
    }



}