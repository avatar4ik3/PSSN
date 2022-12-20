using System.Collections;

namespace PSSN.Core;

public static class Extensions
{
    public static Func<int, int, bool> AscendingComparer = (v1, v2) => v1 <= v2;

    public static Func<int, int> Ascend = v1 => v1 + 1;
    public static Func<int, int> Descend = v1 => v1 - 1;
    public static Func<int, int, bool> DescendingComparer = (v1, v2) => v1 >= v2;

    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }
}

public class RangeEnumerator : IEnumerator<int>
{
    private Func<int, int, bool> _comparer;

    private int _end;

    private Func<int, int> _increment;
    private Range _range;

    public RangeEnumerator(Range range)
    {
        Init(range);
    }

    public int Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        Current = _increment(Current);
        return _comparer(Current, _end);
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