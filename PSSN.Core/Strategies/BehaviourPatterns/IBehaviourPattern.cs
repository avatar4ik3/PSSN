using PSSN.Core.Round;
namespace PSSN.Core.Strategies.BehabiourPatterns;

public interface IBehaviourPattern
{
    IntWrapper[] Coeffs { get; set; }

    Presentation CrossingOverPresentation {get;}
    Presentation MutationPresentation {get;}
    public void Apply(Game g, Player p, ConditionalStrategy s);

    public IBehaviourPattern Copy();
}

public class Presentation {
    public IntWrapper[] Source {get;set;}
    public IntWrapper[] Indexies {get; init;}
}

public class IntWrapper{
    public int Value { get; set; }

    public IntWrapper(int value)
    {
        Value = value;
    }
}