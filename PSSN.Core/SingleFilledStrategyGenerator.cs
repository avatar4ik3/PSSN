using PSSN.Core.Strategies;

namespace PSSN.Core;

public class SingleFilledStrategyGenerator
{
    private static Random? _random;
    private readonly int _stagesCount;

    public SingleFilledStrategyGenerator(int stagesCount,Random random)
    {
        _stagesCount = stagesCount;
        _random = random;
    }

    public FilledStrategy Generate()
    {
        var behaviours = Enumerable.Range(0, _stagesCount).ToDictionary(b => b, b => _random?.NextDouble() < 0.5 ? Behavior.C : Behavior.D);

        return new FilledStrategy(behaviours);
    }

    public FilledStrategy Generate(string name)
    {
        var behaviours = Enumerable.Range(0, _stagesCount).ToDictionary(b => b, b => _random?.NextDouble() < 0.5 ? Behavior.C : Behavior.D);

        return new FilledStrategy(behaviours, name);
    }
}