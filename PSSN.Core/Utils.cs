using PSSN.Core.Strategies;

namespace PSSN.Core;

public static class Utils
{
    public static bool Proc(this Random random, double chance)
    {
        return random.NextDouble() <= chance;
    }

    public static IEnumerable<FilledStrategy> Copy(this IEnumerable<FilledStrategy> strats)
    {
        return strats.Select(x => new FilledStrategy(x.behaviours, x.Name));
    }
}