using PSSN.Core.Strategies;

namespace PSSN.Core;

public class FilledStrategiesGenerator
{
    private readonly int _count;
    private readonly SingleFilledStrategyGenerator _generator;

    public FilledStrategiesGenerator(int count, SingleFilledStrategyGenerator generator)
    {
        this._count = count;
        this._generator = generator;
    }

    public IEnumerable<FilledStrategy> Generate()
    {
        foreach (var current in .._count)
        {
            yield return _generator.Generate(current.ToString());
        }
    }
}
