using PSSN.Core.Round;

namespace PSSN.Core.Strategies.BehabiourPatterns;

public class CttPattern : IBehaviourPattern
{
    public int[]? Coeffs { get; set; } = null;

    public CttPattern(int[] coeffs)
    {
        Coeffs = coeffs;
    }


    public void Apply(Game g, Player p, ConditionalStrategy s)
    {
        if (g.State.currentStage == 0)
        {
            s.Behaviours[g.State.currentStage] = Behavior.C;
            return;
        }
        if (Coeffs is null || Coeffs.Length == 0 || Coeffs[0] < g.State.currentStage)
        {
            s.Behaviours[g.State.currentStage] = g.State.GetOpponentPlayerState(p).previousBehaviours.Last();
            return;
        }
        else if (Coeffs[0] >= g.State.currentStage)
        {
            s.Behaviours[g.State.currentStage] = Behavior.D;
            return;
        }
    }
}