using System.Reflection;
using PSSN.Core.Strategies;

namespace PSSN.Core;

public class StrategesContainer
{
    private readonly Dictionary<string, IStrategy> _strategies = new Dictionary<string, IStrategy>();

    public StrategesContainer()
    {
        var instances =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where typeof(IStrategy).IsAssignableFrom(type) == true
            where typeof(EmptyStrategy).IsAssignableFrom(type) == false
            where type.IsAbstract is false
            where type.IsInterface is false
            select Activator.CreateInstance(type) as IStrategy;


        foreach (var instance in instances)
        {
            _strategies.Add(instance.Name, instance);
        }


    }

    public IStrategy this[string key]
    {
        get => _strategies[key];
    }
}