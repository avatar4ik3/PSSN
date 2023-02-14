using PSSN.Core.Matricies;
using PSSN.Core.States;

namespace PSSN.Core.Round;

public class Game : IDisposable
{
    private readonly double[][] _payoffs;

    public Game(GameState initialState, double[][] payoffs)
    {
        State = initialState;
        _payoffs = payoffs;
    }

    public GameState State { get; set; }

    public void Play()
    {
        foreach (var (s1, s2, s) in State.States(this))
        {
            var payoffs = GetPayoffs();
            s1.Scores[s] = payoffs[s1.p];
            s2.Scores[s] = payoffs[s2.p];
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

    public void Dispose()
    {
        State.Dispose();
    }
}