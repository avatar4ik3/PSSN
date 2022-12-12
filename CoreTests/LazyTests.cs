using PSSN.Core;
using PSSN.Core.Operators;
using PSSN.Core.Round;

namespace PSSN.CoreTests;

public class LazyTests
{
    [Fact]
    public void TestName1()
    {
        // Given
        var random = new Random();
        var population = new FilledStrategiesGenerator(10, new(6, random)).Generate();
        var gameRunner = new SimpleGameRunner();
        // When
        var results = gameRunner.Play(
            population,
            new double[][]{
                new double []{4,0},
                new double[]{6,1}
            }, 6);
        // Then
        var selectionOpeerator = new SelectionOperator(k: 4, random, results);
        var selected1 = selectionOpeerator.Operate(population);
        var selected2 = selectionOpeerator.Operate(population);

        var swaped = new BestScorePickerCrossingOverOperator(6, population, results).Operate(selected1, selected2).ToList();

        var confirmManually = true;
    }
}