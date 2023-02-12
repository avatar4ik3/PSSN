using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.Operators;

public class BestScorePickerCrossingOverOperator
{
    private readonly IEnumerable<IStrategy> _all;
    private readonly int _crossingSelector;
    private readonly TreeGameRunnerResult _results;

    public BestScorePickerCrossingOverOperator(int crossingSelector, IEnumerable<IStrategy> all,
        TreeGameRunnerResult results)
    {
        _crossingSelector = crossingSelector;
        _all = all;
        _results = results;
    }

    public IEnumerable<IStrategy> Operate(IStrategy st1, IStrategy st2)
    {
        if (st1 is not FilledStrategy s1 || st2 is not FilledStrategy s2)
            throw new NotSupportedException($"strats {nameof(st1)} and {nameof(st2)} must be FilledStrats!");
        //берем все стратегии, кроме переданых
        var rest = _all;
        //индексы которые будем свапать
        List<int> indexiesToSwapIntoS1 = new();
        List<int> indexiesToSwapIntoS2 = new();
        //суммы очков за раунд у стратегий s1 s2
        Dictionary<int, double> s1Sums = new();
        Dictionary<int, double> s2Sums = new();

        foreach (var round in s1.behaviours.Keys)
        {
            s1Sums[round] = rest.Where(s => s != s1).Select(s => _results[s1, s, round]).Sum();
            s2Sums[round] = rest.Where(s => s != s1).Select(s => _results[s2, s, round]).Sum();
        }

        //разница в очках. абсолютное значение
        Dictionary<int, double> increment = new();
        foreach (var round in s1.behaviours.Keys) increment[round] = Math.Abs(s1Sums[round] - s2Sums[round]);

        //сортируем инкримент по разнице в очках по убыванию

        var incrementList = increment
            .Select(k => new { index = k.Key, diff = k.Value })
            .OrderByDescending(r => r.diff);
        //смотрим какие гены менять

        foreach (var record in incrementList)
            if (s1Sums[record.index] > s2Sums[record.index])
                indexiesToSwapIntoS2.Add(record.index);
            else
                indexiesToSwapIntoS1.Add(record.index);
        //берем максимум crossingSelector индексов
        //s1 -> s2
        var s1Take = indexiesToSwapIntoS2.Take(_crossingSelector);
        //s2 -> s1
        var s2Take = indexiesToSwapIntoS1.Take(_crossingSelector);

        //создаем новые индексы
        var newS1dic = new Dictionary<int, Behavior>();
        var newS2dic = new Dictionary<int, Behavior>();
        foreach (var index in s1.behaviours.Keys)
        {
            newS1dic[index] = s1.behaviours[index];
            newS2dic[index] = s2.behaviours[index];
        }

        foreach (var index in s1Take) newS2dic[index] = s1.behaviours[index];
        foreach (var index in s2Take) newS1dic[index] = s2.behaviours[index];

        var newS1 = new FilledStrategy(newS1dic, s1.Name);
        var newS2 = new FilledStrategy(newS2dic, s2.Name);
        return new[] { newS1, newS2 };
    }
}