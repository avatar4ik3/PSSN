using PSSN.Core.Round;
namespace PSSN.Core.Strategies.BehabiourPatterns;

public interface IBehaviourPattern
{
    int[]? Coeffs { get; set; }
    public void Apply(Game g, Player p, ConditionalStrategy s);
}