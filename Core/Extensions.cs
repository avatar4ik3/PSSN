using System.Collections;

namespace PSSN.Core;


public static class Extensions
{
    public static RangeEnumerator GetEnumerator(this Range range)
    {
        return new RangeEnumerator(range);
    }

    public static Func<int, int, bool> AscendingComparer = (v1, v2) => v1 <= v2;

    public static Func<int, int> Ascend = (v1) => v1 + 1;
    public static Func<int, int> Descend = (v1) => v1 - 1;
    public static Func<int, int, bool> DescendingComparer = (v1, v2) => v1 >= v2;

}

public class RangeEnumerator : IEnumerator<int>
{
    private Range _range;

    private int _current = 0;
    private int _end;

    private Func<int, int, bool> _comparer;

    private Func<int, int> _increment;


    private void Init(Range range)
    {
        this._range = range;
        _end = range.End.Value;
        if (range.Start.Value < range.End.Value)
        {
            _comparer = Extensions.AscendingComparer;
            _increment = Extensions.Ascend;
            _current = range.Start.Value - 1;

        }
        else
        {
            _comparer = Extensions.DescendingComparer;
            _increment = Extensions.Descend;
            if (range.End.IsFromEnd)
            {
                _end = 0;
            }
            _current = range.Start.Value + 1;
        }
    }

    public RangeEnumerator(Range range)
    {
        Init(range);
    }

    public int Current => _current;

    object IEnumerator.Current => _current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        _current = _increment(_current);
        return _comparer(_current, _end);
    }

    public void Reset()
    {
        Init(_range);
    }
}
