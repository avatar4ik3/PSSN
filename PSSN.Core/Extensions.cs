using System.Collections;
using PSSN.Core.Strategies;

namespace PSSN.Core;

public static class Extensions
{

    public static readonly Func<int, int> Ascend = v1 => v1 + 1;
    public static readonly Func<int, int> Descend = v1 => v1 - 1;
    public static readonly Func<int, int, bool> DescendingComparer = (v1, v2) => v1 >= v2;

    public static readonly Func<int, int, bool> AscendingComparer = (v1, v2) => !DescendingComparer(v1, v2);


    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }


    public static bool Proc(this Random random, double chance)
    {
        return random.NextDouble() <= chance;
    }


    public static IEnumerable<ConditionalStrategy> Copy(this IEnumerable<ConditionalStrategy> strats)
    {
        return strats.Select(x => new ConditionalStrategy() { Pattern = x.Pattern, Behaviours = x.Behaviours, Name = x.Name });
    }

}

public class RangeEnumerator : IEnumerator<int>
{
    private Func<int, int, bool>? _comparer;
    private Func<int, int>? _increment;
    private Range _range;
    private int _end;

    public int Current { get; private set; }

    object IEnumerator.Current => Current;

    public RangeEnumerator(Range range)
    {
        Init(range);
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        Current = _increment!(Current);
        return _comparer!(Current, _end);
    }

    public void Reset()
    {
        Init(_range);
    }

    private void Init(Range range)
    {
        _range = range;
        _end = range.End.Value;
        if (range.Start.Value < range.End.Value)
        {
            _comparer = Extensions.AscendingComparer;
            _increment = Extensions.Ascend;
            Current = range.Start.Value - 1;
        }
        else
        {
            _comparer = Extensions.DescendingComparer;
            _increment = Extensions.Descend;
            if (range.End.IsFromEnd) _end = 0;
            Current = range.Start.Value + 1;
        }
    }
}