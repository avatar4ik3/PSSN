using FluentAssertions;

using Moq;

using PSSN.Core.States;

namespace PSSN.Core.Tests;

public class GameTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var ps1 = new Mock<IPlayerState>();
        var ps2 = new Mock<IPlayerState>();
        ps1.SetupAllProperties();
        ps2.SetupAllProperties();
        ps1.SetupGet(ps1 => ps1.currentBehaviour).Returns(Behavior.C);
        ps2.SetupGet(ps2 => ps2.currentBehaviour).Returns(Behavior.C);
        ps1.SetupGet(ps1 => ps1.previousBehaviours).Returns(new List<Behavior>());
        ps2.SetupGet(ps2 => ps2.previousBehaviours).Returns(new List<Behavior>());
        var expectedCount = 5;
        var state = new GameState(ps1.Object, ps2.Object, expectedCount);

        var realCount = 0;
        // When
        while (state.IsOver() is false)
        {
            state.Next(null!);
            realCount++;
        }
        // Then

        realCount.Should().Be(expectedCount);
    }
}