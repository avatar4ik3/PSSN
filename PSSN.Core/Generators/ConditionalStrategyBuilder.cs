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

    public static int[] buildRandomCoefs(Random random, string type, int gameLength)
    {
        if (type == "MemePattern")
        {
            var res_0 = new[] { random.Next(1, gameLength) };
            var res_1_2 = new[] { random.Next(0, 2), random.Next(0, 2) };
            var res_3 = new[] { random.Next(1, res_0[0] + 1) };
            var res_4_5 = new[] { random.Next(0, 2), random.Next(0, 2) };
            return res_0.Concat(res_1_2).Concat(res_3).Concat(res_4_5).ToArray();
        }
        else
        {
            return new[]{0,0,0,0,0};
            var res = new[] { random.Next(0, gameLength) };
            return res.Concat(Enumerable.Range(0, 4).Select(x => random.Proc(0.5) ? 1 : 0)).ToArray();
        }
    }

    public static IEnumerable<ConditionalStrategy> RandomMemes(Random random, double distr, int count, int gameLength, PatternsContainer container)
    {
        var types = container.PattenrsConstructor.Keys.ToList();
        for (int i = 0; i < count; ++i)
        {
            var type = random.Proc(distr) is true ? types[0] : types[1];

            var coeffs = buildRandomCoefs(random, type, gameLength);

            var pattern = container.CreatePattern(type, coeffs);
            yield return new ConditionalStrategy()
            {
                Pattern = pattern,
                Id = i,
                Name = i.ToString()
            };
        }
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
                Id = i,
                Pattern = selected.First(),
                Name = i.ToString()
            };
        }
    }

    public ConditionalStrategyBuilder WithPattern(IBehaviourPattern pattern)
    {
        _strategy.Pattern = pattern;
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