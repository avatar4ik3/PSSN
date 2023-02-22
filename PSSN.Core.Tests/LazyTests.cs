using PSSN.Core.Generators;
using PSSN.Core.Operators;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class LazyTests
{
    [Fact]
    public void TestName1()
    {
        // Given
        var random = new Random();
        var population = new FilledStrategiesGenerator(10, new SingleFilledStrategyGenerator(6, new Random())).Generate();
        var gameRunner = new SimpleGameRunner();
        // When
        var results = gameRunner.Play(
            population,
            new[]
            {
                new double[] {4, 0},
                new double[] {6, 1}
            }, 6);
        // Then
        var selectionOpeerator = new SelectionOperator<FilledStrategy>(4, results, random);
        var selected1 = selectionOpeerator.Operate(population);
        var selected2 = selectionOpeerator.Operate(population);

        var swaped = new BestScorePickerCrossingOverOperator(6, population, results).Operate(selected1, selected2)
            .ToList();

        var confirmManually = true;
    }
}