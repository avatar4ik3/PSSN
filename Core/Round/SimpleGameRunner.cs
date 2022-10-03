using PSSN.Core.Strategies;
using Combinatorics;
using Combinatorics.Collections;

namespace PSSN.Core.Round;

public class SimpleGameRunner : IGameRunner
{
    public SimpleGameRunner()
    {
        
    }

    public IEnumerable<GameRunnerResult> Play(IEnumerable<IStrategy> strategies,double[][] payoffs)
    {
        List<GameRunnerResult> results = new List<GameRunnerResult>();
        var combos = new Combinations<IStrategy>(strategies,2, GenerateOption.WithRepetition);
        foreach(var combo in combos){
            var s1 = combo[0];
            var s2 = combo[1];
            var game = new Game(
                new States.PlayerState(Player.P1,s1),
                new States.PlayerState(Player.P2,s2),
                payoffs
            );
            game.Play();
            var result = new GameRunnerResult(
                    s1,
                    s2,
                    game.getP1TotalScore(),
                    game.getP2TotalScore()
            );
            results.Add(result);
        }
        return results;
    }

    
}