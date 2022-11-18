using PSSN.Core;
using PSSN.Core.Strategies;
using PSSN.Core.Operators;
using FluentAssertions;

namespace PSSN.CoreTests;

public class CrossingOverTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var s1 = new FilledStrategy(new Dictionary<int, Behavior>(new[]{
            new KeyValuePair<int, Behavior>(0, Behavior.D),
            new(1, Behavior.D),
            new(2, Behavior.D),
            new(3, Behavior.D)
            }
        ));

        var s2 = new FilledStrategy(new Dictionary<int, Behavior>(new[]{
            new KeyValuePair<int, Behavior>(0, Behavior.C),
            new(1, Behavior.C),
            new(2, Behavior.C),
            new(3, Behavior.C)
            }
        ));
        // When
        var op = new DefaultCrossingOverOperator(2);
        var res = op.Operate(s1, s2);
        // Then
        var r1 = (FilledStrategy)res.ElementAt(0);
        var r2 = (FilledStrategy)res.ElementAt(1);

        r1.behaviours.Values.Should().ContainInOrder(Behavior.D, Behavior.D, Behavior.C, Behavior.C);
        r2.behaviours.Values.Should().ContainInOrder(Behavior.C, Behavior.C, Behavior.D, Behavior.D);

    }

    [Fact]
    public void TestName1()
    {
        // Given
        var s1 = new FilledStrategy(new Dictionary<int, Behavior>(new[]{
            new KeyValuePair<int, Behavior>(0, Behavior.D),
            new(1, Behavior.D),
            new(2, Behavior.D),
            new(3, Behavior.D)
            }
        ));

        var s2 = new FilledStrategy(new Dictionary<int, Behavior>(new[]{
            new KeyValuePair<int, Behavior>(0, Behavior.C),
            new(1, Behavior.C),
            new(2, Behavior.C),
            new(3, Behavior.C)
            }
        ));
        // When
        var op = new DefaultCrossingOverOperator(3);
        var res = op.Operate(s1, s2);
        // Then
        var r1 = (FilledStrategy)res.ElementAt(0);
        var r2 = (FilledStrategy)res.ElementAt(1);

        r1.behaviours.Values.Should().ContainInOrder(Behavior.D, Behavior.C, Behavior.C, Behavior.C);
        r2.behaviours.Values.Should().ContainInOrder(Behavior.C, Behavior.D, Behavior.D, Behavior.D);

    }
}