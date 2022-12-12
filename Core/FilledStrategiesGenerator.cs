using PSSN.Core.Strategies;

namespace PSSN.Core;

public class FilledStrategiesGenerator
{
    private readonly int _count;
    private readonly SingleFilledStrategyGenerator _generator;

    public FilledStrategiesGenerator(int count, SingleFilledStrategyGenerator generator)
    {
        if (count <= 1) throw new ArgumentException($"{nameof(count)} cannot be negative or one");
        this._count = count;
        this._generator = generator;
    }

    public IEnumerable<FilledStrategy> Generate()
    {
        foreach (var current in ..(_count - 1))
        {
            yield return _generator.Generate(current.ToString());
        }
    }
}