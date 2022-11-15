using FluentAssertions;
using PSSN.Core;

namespace PSSN.CoreTests;

public class RangeIteratorTests
{
    [Fact]
    public void Iterator_AscendingOrder_1()
    {
        // Given
        var collection = new List<int>();

        // When
        foreach (var i in ..5)
        {
            collection.Add(i);
        }
        // Then
        collection.ToList().Should()
            .BeInAscendingOrder()
            .And
            .ContainInOrder(0, 1, 2, 3, 4, 5);
    }

    [Fact]
    public void Iterator_AscendingOrder_2()
    {
        // Given
        var collection = new List<int>();

        // When
        foreach (var i in 2..5)
        {
            collection.Add(i);
        }
        // Then
        collection.ToList().Should()
            .BeInAscendingOrder()
            .And
            .ContainInOrder(2, 3, 4, 5);
    }

    [Fact]
    public void Iterator_DescendingOrder_2()
    {
        // Given
        var collection = new List<int>();

        // When
        foreach (var i in 5..0)
        {
            collection.Add(i);
        }
        // Then
        collection.Should()
            .BeInDescendingOrder()
            .And
            .ContainInOrder(5, 4, 3, 2, 1, 0);
    }

    [Fact]
    public void Iterator_DescendingOrder_1()
    {
        // Given
        var collection = new List<int>();

        // When
        foreach (var i in 5..)
        {
            collection.Add(i);
        }
        // Then
        collection.Should()
            .BeInDescendingOrder()
            .And
            .ContainInOrder(5, 4, 3, 2, 1, 0);
    }
}