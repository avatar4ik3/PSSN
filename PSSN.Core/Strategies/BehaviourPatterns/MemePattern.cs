using PSSN.Core.Round;

namespace PSSN.Core.Strategies.BehabiourPatterns;

public class MemePattern : IBehaviourPattern
{
    public IntWrapper[] Coeffs { get; set; }

    public Presentation CrossingOverPresentation { get; }

    public Presentation MutationPresentation { get; }

    public MemePattern(int[] coeffs)
    {
        this.Coeffs = coeffs.Select(x => new IntWrapper(x)).ToArray();
        CrossingOverPresentation = new()
        {
            Source = Coeffs,
            Indexies = Coeffs,
        };
        MutationPresentation = new()
        {
            Source = Coeffs,
            Indexies = new IntWrapper[] { new(1), new(5) }
        };
    }

    /*
     --- набор истории   ---
    0 - (int) = выбранное количество раундов
    1 - (0/1), — играть c/d <- [1] начальное поведение
    --- условие --- 
    2 - (0/1), (>=/<=), —  = 0 если противник сыграл >=    
    3 - (int),— = количества
    4 - (0/1), — c или d  
    --- то  ---
    5 - (0/1), то играть : c иначе d / d иначе с
    */
    public void Apply(Game g, Player p, ConditionalStrategy s)
    {
        if (Coeffs is null || Coeffs.Length != 5)
        {
            throw new ArgumentException(nameof(Coeffs));
        }
        //набор истории
        if (g.State.currentStage <= Coeffs[0].Value)
        {
            s.Behaviours[g.State.currentStage] = (Behavior)Coeffs[1].Value;
        }

        //условие
        //false = greater
        //true = less
        var lessOrGreater = Coeffs[2].Value is 0;

        //сколько раз противник за выбранный промежуток [3] сыграл [4]
        var a = g.State.GetOpponentPlayerState(p).previousBehaviours.Skip(g.State.currentStage - Coeffs[0].Value).Where(x => (int)x == Coeffs[4].Value).Count();
        if (
            (a >= Coeffs[3].Value) == lessOrGreater
        )
        {
            s.Behaviours[g.State.currentStage] = (Behavior)Coeffs[5].Value;
        }
        else
        {
            s.Behaviours[g.State.currentStage] = (Behavior)(Coeffs[5].Value is 0 ? 1 : 0);
        }
    }

    public IBehaviourPattern Copy()
    {
        var arr = new IntWrapper[6];
        Coeffs.CopyTo(arr, 0);
        return new MemePattern(arr.Select(x => x.Value).ToArray());
    }
}