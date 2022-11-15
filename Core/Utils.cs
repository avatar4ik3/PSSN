namespace PSSN.Core;

public static class Utils
{
    public static bool Proc(this Random random, double chance)
    {
        return random.NextDouble() <= chance;
    }
}