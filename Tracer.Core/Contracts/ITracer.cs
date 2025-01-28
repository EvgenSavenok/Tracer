namespace Tracer.Coree.Contracts;

public interface ITracer
{
    void StartTrace();

    void StopTrace();

    TraceResult GetTraceResult();
}