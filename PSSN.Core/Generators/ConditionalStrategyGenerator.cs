using PSSN.Core.Strategies;

namespace PSSN.Core.Generators;

public class ConditionalStrategyGenerator
{
    private readonly int _count;
    private readonly SingleConditionalStrategyGenerator _generator;

    public ConditionalStrategyGenerator(int count, SingleConditionalStrategyGenerator generator)
    {
        this._count = count;
        this._generator = generator;
    }

    public IEnumerable<ConditionalStrategy> Generate()
    {
        foreach (var i in 0.._count)
        {
            yield return _generator.Generate(i.ToString());
        }
    }
}