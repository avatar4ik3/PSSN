using PSSN.Core.Matrices;
using PSSN.Core.States;

namespace PSSN.Core.Round;

public class Game
{
    private readonly double[][] _payoffs;

    public GameState State { get; set; }

    public Game(PlayerState ps1, PlayerState ps2,double[][] payoffs)
    {
        State = new GameState(ps1, ps2);
        this._payoffs = payoffs;
    }

    public void Play()
    {
        while (State.Next(this) is false)
        {
            PlayerPayoffRoundMap payoffs = GetPayoffs();
            State.ps1.scores += (payoffs[State.ps1.p]);
            State.ps2.scores += (payoffs[State.ps2.p]);
        }
    }

    private PlayerPayoffRoundMap GetPayoffs()
    {
        var b1 = State.ps1.currentBehaviour;
        var b2 = State.ps2.currentBehaviour;
        var win1 = _payoffs[(int)b1][(int)b2];
        var win2 = _payoffs[(int)b2][(int)b1];
        var map = new PlayerPayoffRoundMap();
        map[State.ps1.p] = win1;
        map[State.ps2.p] = win2;
        return map;
    }

    public double getP1TotalScore(){
        return State.ps1.scores;
    }

    public double getP2TotalScore(){
        return State.ps2.scores;
    }
}