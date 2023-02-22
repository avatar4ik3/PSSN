using PSSN.Core.Containers;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class StratifiesContainerTests
{
    ///хардок для проверки работоспособности
    [Fact]
    public void TestName()
    {
        // Given
        var container = new StrategiesContainer();
        // When

        // Then
        Assert.Equal("C", container["C"].Name);
        Assert.Equal("D", container["D"].Name);
        Assert.Equal("CD", container["CD"].Name);
        Assert.Equal("CTT", container["CTT"].Name);
        Assert.Equal("CTT3D", container["CTT3D"].Name);
        Assert.Equal("CTT4D", container["CTT4D"].Name);
        Assert.Equal("CTT5D", container["CTT5D"].Name);
        Assert.Equal("CTT6D", container["CTT6D"].Name);

        Assert.Equal(container["CD"], new CD());
    }
}