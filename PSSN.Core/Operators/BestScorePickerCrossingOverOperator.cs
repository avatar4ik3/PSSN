using System;
using System.Collections.Generic;
using System.Linq;
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

    public IEnumerable<ConditionalStrategy> Operate(ConditionalStrategy s1, ConditionalStrategy s2)
    {
        //берем все стратегии, кроме переданых
        var rest = _all.Where(s => s != s1 && s != s2).ToList();
        //индексы которые будем свапать
        List<int> indexiesToSwapIntoS1 = new();
        List<int> indexiesToSwapIntoS2 = new();
        //суммы очков за раунд у стратегий s1 s2
        Dictionary<int, double> s1Sums = new();
        Dictionary<int, double> s2Sums = new();

        foreach (var round in s1.Behaviours.Keys)
        {
            s1Sums[round] = rest.Select(s => _results[s1, s, round]).Sum();
            s2Sums[round] = rest.Select(s => _results[s2, s, round]).Sum();
        }

        //разница в очках. абсолютное значение
        Dictionary<int, double> increment = new();
        foreach (var round in s1.Behaviours.Keys) increment[round] = Math.Abs(s1Sums[round] - s2Sums[round]);

        //сортируем инкримент по разнице в очках по убыванию

        var incrementList = increment
            .Select(k => new { index = k.Key, diff = k.Value })
            .OrderByDescending(r => r.diff);
        //смотрим какие гены менять

        foreach (var index in incrementList.Select(x => x.index))
            if (s1Sums[index] > s2Sums[index])
                indexiesToSwapIntoS2.Add(index);
            else
                indexiesToSwapIntoS1.Add(index);
        //берем максимум crossingSelector индексов
        //s1 -> s2
        var s1Take = indexiesToSwapIntoS2.Take(_crossingSelector);
        //s2 -> s1
        var s2Take = indexiesToSwapIntoS1.Take(_crossingSelector);

        //создаем новые индексы
        var newS1dic = new Dictionary<int, Behavior>();
        var newS2dic = new Dictionary<int, Behavior>();
        foreach (var index in s1.Behaviours.Keys)
        {
            newS1dic[index] = s1.Behaviours[index];
            newS2dic[index] = s2.Behaviours[index];
        }

        foreach (var index in s1Take) newS2dic[index] = s1.Behaviours[index];
        foreach (var index in s2Take) newS1dic[index] = s2.Behaviours[index];

        var newS1 = new ConditionalStrategy() { Pattern = s1.Pattern, Behaviours = newS1dic, Name = s1.Name };
        var newS2 = new ConditionalStrategy() { Pattern = s1.Pattern, Behaviours = newS2dic, Name = s2.Name };
        return new[] { newS1, newS2 };
    }
}