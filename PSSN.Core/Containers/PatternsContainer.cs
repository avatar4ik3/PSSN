using PSSN.Core.Strategies.BehabiourPatterns;

namespace PSSN.Core.Containers;

public class PatternsContainer
{
    public Dictionary<string, Func<int[], IBehaviourPattern>> PattenrsConstructor { get; init; } = new();

    public PatternsContainer()
    {
        var acceptedTypes = new[] { typeof(int[]) };
        var instances =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where typeof(IBehaviourPattern).IsAssignableFrom(type)
            where type.IsInterface is false
            select (t: type, ctor: type.GetConstructor(acceptedTypes));

        foreach (var instance in instances)
        {
            PattenrsConstructor[instance.t.Name] = (int[] coefs) => (instance.ctor.Invoke(new object[] { coefs }) as IBehaviourPattern)!;
        }
    }

    public IBehaviourPattern CreatePattern(string name, int[] coeffs)
    {
        return PattenrsConstructor[name].Invoke(coeffs);
    }
}