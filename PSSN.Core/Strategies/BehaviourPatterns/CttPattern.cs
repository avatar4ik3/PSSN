using PSSN.Core.Round;

namespace PSSN.Core.Strategies.BehabiourPatterns;

public class CttPattern : IBehaviourPattern
{
    public IntWrapper[] Coeffs { get; set; } = null;

    // public ref int I1 => ref Coeffs![1];
    // public ref int I2 => ref Coeffs![4];
    public Presentation CrossingOverPresentation { get; }

    public Presentation MutationPresentation { get; }
    public CttPattern(int[] coeffs)
    {
        Coeffs = coeffs.Select(x => new IntWrapper(x)).ToArray();
        CrossingOverPresentation = new()
        {
            Source = Coeffs,
            Indexies = new IntWrapper[] { new(1), Coeffs[1], new(0), new(1), new(0), Coeffs[4] },
        };
        MutationPresentation = new()
        {
            Source = Coeffs,
            Indexies = new IntWrapper[] { new(1), new(4) },
        };

    }


    /*
    есть
    0 - (int), количество раундов на котором будет играться ctt
    1 - (0/1), c/d начальное поведение
    2 - (0/1), c/d конечное поведение
    3 - (0/1), включить ли CTT поведение
    4 - (0/1), является ли reverse 
    
    Отображение в условные стратегии
    --- перекодировка из передыдущего решения чтобы нихуя не посыпалось
    
    --- набор истории   ---
    0 - (int) = 1 — первый раунд
    1 - (0/1), — играть c/d <- [1] начальное поведение
    --- условие --- 
    2 - (0/1), (>=/<=), —  = 0 если противник сыграл >=    
    3 - (int),— = 1 
    4 - (0/1), — c или d - [4] является ли reverse 
    --- то  ---
    5 - (0/1), то c иначе d / то d иначе с
    
    отображения
    1 - CrossingOver

        строится отображение на условные стратегии

        0 - (int) =1
        1 - [1]
        2 - 0
        3 - 1
        4 - 0
        5 - [4]


        используется breaking index = 2
        <2  -   этап набора истории
            Отсюда мы берем индекс 0 - количество раундов которые игрался индекс 1, и смотрим его очки
        >=2 -   этап выбора решения
            Количество раундов, которые игралось это поведение = длина генотипа - количество раундов которые игрался индекс 1
        
        Берутся очки за каждый из этапов. свапается наилучший
    
    2 - Mutation 

        используется маппинг -  возвращается массив [1,5] - булевые числа, они мутируют с 0 -> 1 / 1 -> 0
        и записываются обратно
    
    */


    public void Apply(Game g, Player p, ConditionalStrategy s)
    {
        if (Coeffs is null || Coeffs.Length != 5)
        {
            throw new ArgumentException(nameof(Coeffs));
        }
        if (g.State.currentStage == 0)
        {
            s.Behaviours[g.State.currentStage] = (Behavior)Coeffs[1].Value;
            return;
        }
        if (Coeffs[3].Value == 0 || g.State.currentStage + 1 < Coeffs[0].Value)
        {
            var beh = g.State.GetOpponentPlayerState(p).previousBehaviours.Last();
            s.Behaviours[g.State.currentStage] = Coeffs[4].Value == 1 ? beh.Other() : beh;
            return;
        }
        else if (Coeffs[3].Value == 1 && g.State.currentStage + 1 >= Coeffs[0].Value)
        {
            s.Behaviours[g.State.currentStage] = (Behavior)Coeffs[2].Value;
            return;
        }
    }

    public IBehaviourPattern Copy()
    {
        var arr = new IntWrapper[5];
        Coeffs.CopyTo(arr,0);
        return new CttPattern(arr.Select(x => x.Value).ToArray());
    }
}