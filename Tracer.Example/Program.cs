using Tracer.Coree.Contracts;
using TracerSerialization;

namespace Tracer.Example;

class Program
{
    static void Main()
    {
        ITracer tracer = new Tracer();

        var threads = new Thread[5];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                NestedMethod(tracer);
                NestedMethod(tracer);
            });
            threads[i].Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }

        var traceResult = tracer.GetTraceResult();

        var serializers = PluginLoader.LoadSerializers("SerializationPlugins");
        foreach (var serializer in serializers)
        {
            using var stream = File.Create($"result.{serializer.Format}");
            serializer.Serialize(traceResult, stream);
        }
        Console.WriteLine("Tracing completed and results saved.");
    }
    
    static void NestedMethod(ITracer tracer)
    {
        tracer.StartTrace();
        Thread.Sleep(150);
        tracer.StopTrace();
    }
}