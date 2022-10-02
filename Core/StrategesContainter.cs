using System.Reflection;
using PSSN.Core.Strategies;

namespace PSSN.Core;

public class StrategesContainter
{
    private readonly Dictionary<string, IStrategy> _strategies = new Dictionary<string, IStrategy>();

    public StrategesContainter()
    {
        var instances = 
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where typeof(IStrategy).IsAssignableFrom(type)
            where type.IsAbstract is false
            where type.IsInterface is false
            select Activator.CreateInstance(type) as IStrategy;

        
        foreach(var instance in instances){
            _strategies.Add(instance.Name,instance);
        }
        
        
    }

    public IStrategy this[string key]{
        get => _strategies[key];
    }
}