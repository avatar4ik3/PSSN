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
        if (Coeffs is null || Coeffs.Length != 5)
        {
            throw new ArgumentException();
        }
        if (g.State.currentStage == 0)
        {
            s.Behaviours[g.State.currentStage] = (Behavior)Coeffs[1];
            return;
        }
        if (Coeffs[3] == 0 || g.State.currentStage + 1 < Coeffs[0])
        {
            var beh = g.State.GetOpponentPlayerState(p).previousBehaviours.Last();
            s.Behaviours[g.State.currentStage] = Coeffs[4] == 1 ? beh.Other() : beh;
            return;
        }
        else if (Coeffs[3] == 1 && g.State.currentStage + 1 >= Coeffs[0])
        {
            s.Behaviours[g.State.currentStage] = (Behavior)Coeffs[2];
            return;
        }
    }
}