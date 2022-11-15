using PSSN.Core.Strategies;

namespace PSSN.Core;

public class SingleFilledStrategyGenerator
{
    private readonly int _stagesCount;
    private readonly Random _random;

    public SingleFilledStrategyGenerator(int stagesCount, Random random)
    {
        this._stagesCount = stagesCount;
        this._random = random;
    }

    public FilledStrategy Generate()
    {
        var behaviours = new Dictionary<int, Behavior>();

        foreach (var b in Enumerable.Range(0, _stagesCount))
        {
            behaviours.Add(
                b,
                _random.NextDouble() < (0.5) ? Behavior.C : Behavior.D
            );
        }

        return new FilledStrategy(behaviours);
    }

    public FilledStrategy Generate(string name)
    {
        var behaviours = new Dictionary<int, Behavior>();

        foreach (var b in Enumerable.Range(0, _stagesCount))
        {
            behaviours.Add(
                b,
                _random.NextDouble() < (0.5) ? Behavior.C : Behavior.D
            );
        }

        return new FilledStrategy(behaviours, name);
    }
}