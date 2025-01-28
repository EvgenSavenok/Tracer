using System.Reflection;
using Tracer.Serialization.Abstraction;

namespace TracerSerialization;

public class PluginLoader
{
    public static IEnumerable<ITraceResultSerializer> LoadSerializers(string pluginsPath)
    {
        var serializers = new List<ITraceResultSerializer>();
        foreach (var file in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(file);
            var types = assembly.GetTypes()
                .Where(t => typeof(ITraceResultSerializer).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is ITraceResultSerializer serializer)
                {
                    serializers.Add(serializer);
                }
            }
        }
        return serializers;
    }
}