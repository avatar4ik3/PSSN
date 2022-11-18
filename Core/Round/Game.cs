using PSSN.Core.Matrices;
using PSSN.Core.States;

namespace PSSN.Core.Round;

public class Game
{
    private readonly double[][] _payoffs;

    public GameState State { get; set; }

    public Game(IPlayerState ps1, IPlayerState ps2, double[][] payoffs)
    {
        State = new GameState(ps1, ps2);
        this._payoffs = payoffs;
    }

    public void Play()
    {
        while (State.IsOver() is false)
        {
            State.Next(this);
            PlayerPayoffRoundMap payoffs = GetPayoffs();
            State.ps1.Scores[State.currentStage] = (payoffs[State.ps1.p]);
            State.ps2.Scores[State.currentStage] = (payoffs[State.ps2.p]);
        }
    }

    private PlayerPayoffRoundMap GetPayoffs()
    {
        var map = new PlayerPayoffRoundMap(_payoffs);
        map.Calculate(State.ps1, State.ps2);
        return map;
    }

    public Dictionary<int, double> P1Scores()
    {
        return State.ps1.Scores;
    }

    public Dictionary<int, double> P2Scores()
    {
        return State.ps2.Scores;
    }
}