using System.Text.Json;
using Tracer.Serialization.Abstraction;

namespace Tracer.Serialization.Json
{
    public class JsonTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => "json";

        public void Serialize(TraceResult traceResult, Stream output)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            using var writer = new StreamWriter(output);
            writer.Write(JsonSerializer.Serialize(traceResult, options));
        }
    }
}