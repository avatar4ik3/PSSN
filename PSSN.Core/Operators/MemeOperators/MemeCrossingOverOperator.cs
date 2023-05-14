using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.Operators.MemeOperators;

public class MemeCrossingOverOperator
{

    private readonly IEnumerable<IStrategy> _all;
    private readonly TreeGameRunnerResult _results;

    public MemeCrossingOverOperator(IEnumerable<IStrategy> all,
        TreeGameRunnerResult results)
    {

        _all = all;
        _results = results;
    }

    public IEnumerable<ConditionalStrategy> Operate(ConditionalStrategy s1, ConditionalStrategy s2)
    {
        //суммы очков за раунд у стратегий s1 s2
        Dictionary<int, double> s1Sums = new();
        Dictionary<int, double> s2Sums = new();

        // #if !DEBUG
        //берем все стратегии, кроме переданых
        var rest = _all.Where(s => s != s1 && s != s2).ToList();
        foreach (var round in s1.Behaviours.Keys)
        {
            s1Sums[round] = rest.Select(s => _results[s1, s, round]).Sum();
            s2Sums[round] = rest.Select(s => _results[s2, s, round]).Sum();
        }
        // #endif

        //         //для дебага
        // #if DEBUG
        //     s1Sums = new(){
        //         {1,5},
        //         {2,5},
        //         {3,5}
        //     };

        //     s2Sums = new(){
        //         {1,1},
        //         {2,1},
        //         {3,1}
        //     };
        // #endif
        //сплитим 
        //костыль, мы сейчас думаем что у нас только один паттерн
        var cs_1 = s1.Pattern.CrossingOverPresentation;
        var cs_2 = s2.Pattern.CrossingOverPresentation;

        var len_setup_s1 = s1Sums.Values.Take(cs_1.Indexies![0].Value).Count();
        var len_setup_s2 = s2Sums.Values.Take(cs_2.Indexies![0].Value).Count();

        //количество очков первой стратегии за время сетапа
        var sum_setup_s_1 = len_setup_s1 == 0 ? 0 : (s1Sums.Values.Take(cs_1.Indexies![0].Value).Sum() / len_setup_s1);
        //количество очков второй стратегии за время сетапа
        var sum_setup_s_2 = len_setup_s2 == 0 ? 0 : (s2Sums.Values.Take(cs_2.Indexies![0].Value).Sum() / len_setup_s2);

        //разница между полученными очками в сетапе
        var sum_setup_dif = Math.Abs(sum_setup_s_1 - sum_setup_s_2);

        //сколько раундов было после сетапа
        var len_after_s_1 = s1Sums.Values.Skip(cs_1.Indexies[0].Value).Count();
        var len_after_s_2 = s2Sums.Values.Skip(cs_2.Indexies[0].Value).Count();
        //количество очков первой стратегии после сетапа
        var sum_after_s_1 = len_after_s_1 == 0 ? 0 : (s1Sums.Values.Skip(cs_1.Indexies[0].Value).Sum() / len_after_s_1);
        //количество очков второй стратегии после сетапа
        var sum_after_s_2 = len_after_s_2 == 0 ? 0 : (s2Sums.Values.Skip(cs_2.Indexies[0].Value).Sum() / len_after_s_2);


        //разница между полученными очками в после сетапа
        var sum_after_dif = Math.Abs(sum_after_s_1 - sum_after_s_2);

        var res1 = new ConditionalStrategy()
        {
            Behaviours = s1.Behaviours,
            Pattern = s1.Pattern.Copy(),
            Name = s1.Name
        };

        var res2 = new ConditionalStrategy()
        {
            Behaviours = s2.Behaviours,
            Pattern = s2.Pattern.Copy(),
            Name = s2.Name
        };

        var r1 = res1.Pattern.CrossingOverPresentation;
        var r2 = res2.Pattern.CrossingOverPresentation;

        //свапаем только ту часть, в которой разница по очкам максимальна
        // if (sum_after_dif > sum_setup_dif)
        // {
        //если нужно свапать часть после сетапа

        if (sum_after_s_1 > sum_after_s_2)
        {
            for (int i = 2; i < 6; ++i)
            {
                r2.Indexies[i].Value = cs_1.Indexies[i].Value;
            }
        }
        else
        {
            for (int i = 2; i < 6; ++i)
            {
                r1.Indexies[i].Value = cs_2.Indexies[i].Value;
            }
        }
        // }
        // else
        // {
        //если нужно свапать сетап
        if (sum_setup_s_1 > sum_setup_s_2)
        {
            for (int i = 0; i < 2; ++i)
            {
                r2.Indexies[i].Value = cs_1.Indexies[i].Value;
            }
        }
        else
        {
            for (int i = 0; i < 2; ++i)
            {
                r1.Indexies[i].Value = cs_2.Indexies[i].Value;
            }
        }
        // }
        return new[] { res1, res2 };
    }
}