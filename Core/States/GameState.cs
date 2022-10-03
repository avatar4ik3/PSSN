using PSSN.Core.Round;

namespace PSSN.Core.States;

public class GameState : State
{
    public PlayerState ps1 {get;set;}
    public PlayerState ps2 {get;set;}

    public int currentStage{get;set;} = 0;

    public static int maxCountOfStages{get;set;} = 5;

    public GameState(PlayerState ps1, PlayerState ps2)
    {
        this.ps1 = ps1;
        this.ps2 = ps2;
    }


    public PlayerState? GetPlayerStateByPlayer(Player p){
        if(ps1.p.Equals(p))return ps1;
        if(ps2.p.Equals(p))return ps2;
        return null;
    }

    public PlayerState? GetOpponentPlayerState(Player p){
        if(ps1.p.Equals(p))return ps2;
        if(ps2.p.Equals(p))return ps1;
        return null;
    }

    private bool IsOver(){
        var res = currentStage > maxCountOfStages;
        currentStage++;
        return res;
    }
    public bool Next(Game g)
    {
        if(currentStage == 0){
            ps1.Next(g);
            ps2.Next(g);
        }
        else {
            ps1.previousBehaviours.Add(ps1.currentBehaviour);
            ps2.previousBehaviours.Add(ps2.currentBehaviour);

            ps1.Next(g);
            ps2.Next(g);
        }

        return IsOver();
    }
}