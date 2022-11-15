using Moq;
using PSSN.Core.States;
using PSSN.Core;
using PSSN.Core.Operators;
using FluentAssertions;

namespace PSSN.CoreTests;

public class SelectionTests
{
    [Fact]
    public void TestName1()
    {
        // Given
        Random random = new Random();
        var sg = new SingleFilledStrategyGenerator(6, random);
        var generator = new FilledStrategiesGenerator(10, sg);
        List<IPlayerState> states = new();
        foreach (var s in generator.Generate())
        {
            var moq = new Mock<IPlayerState>();
            moq.SetupAllProperties();
            // moq.SetupGet(moq => moq.Scores).Returns(random.NextDouble() * 10);
            moq.Object.Scores = random.NextDouble() * 10;
            moq.Setup(moq => moq.strategy).Returns(s);
            states.Add(moq.Object);
        }

        var op = new SelectionOperator(4, random);

        var result = op.Operate(states);
        // When
        var prev = states.Where(s => s.strategy == result.First()).First().Scores;
        states.Select(s => s.strategy).Should().Contain(result);


        var a = states.Select(s => new { score = s.Scores, strat = s.strategy }).ToList();
        result.Count().Should().Be(1);
        // Then
    }
}