using FluentAssertions;

using PSSN.Core.Matricies;
using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class PayoffsRoundMapTests
{
    [Theory]
    [InlineData(Behavior.C, Behavior.C)]
    [InlineData(Behavior.C, Behavior.D)]
    [InlineData(Behavior.D, Behavior.C)]
    [InlineData(Behavior.D, Behavior.D)]
    public void TestName(Behavior b1, Behavior b2)
    {
        // Given
        var matrix = new[]
        {
            new double[] {4, 0},
            new double[] {6, 1}
        };
        var ps1 = new PlayerState(Player.P1, new CD());
        var ps2 = new PlayerState(Player.P2, new CD());
        ps1.currentBehaviour = b1;
        ps2.currentBehaviour = b2;
        var map = new PlayerPayoffRoundMap(matrix);
        // When
        var res = map.Calculate(ps1, ps2);
        res[Player.P1].Should().Be(matrix[(int) b1][(int) b2]);
        res[Player.P2].Should().Be(matrix[(int) b2][(int) b1]);

        // Then
    }
}