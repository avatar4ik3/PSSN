using PSSN.Core.Strategies;

namespace PSSN.Core.Containers;

public class StrategiesContainer
{
    private readonly Dictionary<string, IStrategy> _strategies = new();

    public StrategiesContainer()
    {
        var instances =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where typeof(IStrategy).IsAssignableFrom(type)
            where typeof(ConditionalStrategy).IsAssignableFrom(type) == false
            where type.IsAbstract is false
            where type.IsInterface is false
            select Activator.CreateInstance(type) as IStrategy;

        foreach (var instance in instances) _strategies.Add(instance.Name, instance);
    }

    public IStrategy this[string key] => _strategies[key];
}