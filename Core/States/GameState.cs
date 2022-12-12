using PSSN.Core.Round;

namespace PSSN.Core.States;

public class GameState
{
    public IPlayerState ps1 { get; init; }
    public IPlayerState ps2 { get; init; }

    public int currentStage { get; private set; } = 0;

    public int maxCountOfStages { get; init; } = 0;

    //TODO переделать наконец в то что количество стадий настраиваемо
    public GameState(IPlayerState ps1, IPlayerState ps2, int countOfStages = 5)
    {
        this.ps1 = ps1;
        this.ps2 = ps2;
        this.maxCountOfStages = countOfStages;
    }


    public IPlayerState? GetPlayerStateByPlayer(Player p)
    {
        if (ps1.p.Equals(p)) return ps1;
        if (ps2.p.Equals(p)) return ps2;
        return null;
    }

    public IPlayerState? GetOpponentPlayerState(Player p)
    {
        if (ps1.p.Equals(p)) return ps2;
        if (ps2.p.Equals(p)) return ps1;
        return null;
    }

    public bool IsOver()
    {
        var res = currentStage >= maxCountOfStages;

        return res;
    }
    public void Next(Game g)
    {
        if (currentStage == 0)
        {
            ps1.Next(g);
            ps2.Next(g);
        }
        else
        {
            ps1.previousBehaviours.Add(ps1.currentBehaviour);
            ps2.previousBehaviours.Add(ps2.currentBehaviour);

            ps1.Next(g);
            ps2.Next(g);
        }
        currentStage++;
    }
}