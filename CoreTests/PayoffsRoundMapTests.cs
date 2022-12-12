using PSSN.Core.Matrices;
using Moq;
using PSSN.Core.States;
using PSSN.Core.Strategies;
using FluentAssertions;
using PSSN.Core;

namespace PSSN.CoreTests;

public class PayoffsRoundMapTests
{
    [Theory]
    [InlineData(Core.Behavior.C, Core.Behavior.C)]
    [InlineData(Core.Behavior.C, Core.Behavior.D)]
    [InlineData(Core.Behavior.D, Core.Behavior.C)]
    [InlineData(Core.Behavior.D, Core.Behavior.D)]
    public void TestName(Core.Behavior b1, Core.Behavior b2)
    {
        // Given
        var matrix = new double[][]{
            new double[]{4,0},
            new double[]{6,1}
        };
        var ps1 = new PlayerState(Core.Player.P1, new CD());
        var ps2 = new PlayerState(Core.Player.P2, new CD());
        ps1.currentBehaviour = b1;
        ps2.currentBehaviour = b2;
        var map = new PlayerPayoffRoundMap(matrix);
        // When
        var res = map.Calculate(ps1, ps2);
        res[Core.Player.P1].Should().Be(matrix[(int)b1][(int)b2]);
        res[Core.Player.P2].Should().Be(matrix[(int)b2][(int)b1]);

        // Then
    }
}