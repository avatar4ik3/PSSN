using FluentAssertions;

using PSSN.Core.Round;
using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class CTTTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var s1 = new CTT6D();
        var s2 = new AlwaysC();

        var poffs = new[]
        {
            new double[] {4, 0},
            new double[] {6, 1}
        };
        var game = new Game(new GameState(new PlayerState(Player.P1, s1), new PlayerState(Player.P2, s2), 6), poffs);
        // When
        game.Play();
        // Then

        var p1s = game.P1Scores().Select(x => x.Value).Sum();
        var p2s = game.P2Scores().Select(x => x.Value).Sum();

        game.P1Scores().Should().HaveCount(6);

        p1s.Should().Be(26);
        p2s.Should().Be(20);
    }
}