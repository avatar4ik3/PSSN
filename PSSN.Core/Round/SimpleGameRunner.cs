using System.Collections.Concurrent;
using Combinatorics.Collections;
using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public class SimpleGameRunner : IGameRunner
{
    public TreeGameRunnerResult Play(IEnumerable<IStrategy> strategies, double[][] payoffs, int rounds)
    {
        TreeGameRunnerResult results = new();
        var combos = new Combinations<IStrategy>(strategies, 2, GenerateOption.WithRepetition);
        foreach (var combo in combos)
        {
            var s1 = combo[0];
            var s2 = combo[1];
            var ps1 = new PlayerState(Player.P1, s1);
            var ps2 = new PlayerState(Player.P2, s2);
            GameState gs = new GameState(
                ps1, ps2,
                rounds);
            var game = new Game(
                gs,
                payoffs
            );
            game.Play();
            results[s1, s2] = game.P1Scores();
            results[s2, s1] = game.P2Scores();
        }
        
        return results;
    }

    public void Dispose()
    {
    }
}