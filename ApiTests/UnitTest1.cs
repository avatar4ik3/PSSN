namespace ApiTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var strats = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var tuples = strats[0..strats.Count()].Zip(strats[1..strats.Count()]);

        var a = 0;

    }
}