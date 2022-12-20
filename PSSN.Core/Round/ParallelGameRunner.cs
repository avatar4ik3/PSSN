using Combinatorics.Collections;

using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public class ParallelGameRunner : IGameRunner
{
    public TreeGameRunnerResult Play(IEnumerable<IStrategy> strategies, double[][] payoffs, int rounds)
    {
        TreeGameRunnerResult results = new();
        var combos = new Combinations<IStrategy>(strategies, 2, GenerateOption.WithRepetition);
        Parallel.ForEach(combos,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            },
            combo =>
            {
                var s1 = combo[0];
                var s2 = combo[1];
                var game = new Game(
                    new GameState(
                        new PlayerState(Player.P1, s1),
                        new PlayerState(Player.P2, s2),
                        rounds
                    ), payoffs
                );
                game.Play();
                results[s1, s2] = game.P1Scores();
                results[s2, s1] = game.P2Scores();
            });
        return results;
    }
}