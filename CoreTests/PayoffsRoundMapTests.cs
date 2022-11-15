using PSSN.Core.Matrices;
using Moq;
using PSSN.Core.States;
using PSSN.Core.Strategies;

namespace PSSN.CoreTests;

public class PayoffsRoundMapTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var matrix = new double[][]{
            new double[]{4,0},
            new double[]{6,1}
        };
        var ps1 = new PlayerState(Core.Player.P1, new CD());
        var ps2 = new PlayerState(Core.Player.P2, new CD());
        ps1.currentBehaviour = Core.Behavior.C;
        ps2.currentBehaviour = Core.Behavior.D;
        var map = new PlayerPayoffRoundMap(matrix);
        // When
        var res = map.Calculate(ps1, ps2);
        var a = 2;
        // Then
    }
}