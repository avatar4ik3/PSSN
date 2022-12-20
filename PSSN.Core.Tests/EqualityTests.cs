using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.Tests;

public class EqualityTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var d1 = new Dictionary<int, Behavior>();
        d1[0] = Behavior.C;
        d1[1] = Behavior.D;
        var s1 = new FilledStrategy(d1, "1");

        var d2 = new Dictionary<int, Behavior>();
        d2[0] = Behavior.C;
        d2[1] = Behavior.D;
        var s2 = new FilledStrategy(d1, "1");

        var tree = new TreeGameRunnerResult();
        tree[s1, s2] = null!;
        var a = tree[s1];
        //tree[s1].Should().ContainKey(s2);
    }
}