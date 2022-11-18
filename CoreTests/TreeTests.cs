using PSSN.Core;
using PSSN.Core.Round;

namespace PSSN.CoreTests;

public class TreeTests
{
    [Fact]
    public void TestName()
    {
        // Given
        Random random = new Random();
        var sg = new SingleFilledStrategyGenerator(6, random);
        var generator = new FilledStrategiesGenerator(10, sg);
        var population = generator.Generate();
        var tree = new TreeGameRunnerResult();
        foreach (var s1 in population)
        {
            foreach (var s2 in population)
            {
                foreach (var r in s1.behaviours.Keys)
                {
                    var val = random.NextDouble() * 10;
                    tree[s1, s2, r] = val;
                }
            }
        }

        var a = 2;
    }
}