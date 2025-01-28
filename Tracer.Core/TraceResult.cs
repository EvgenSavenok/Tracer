namespace Tracer;

public class TraceResult
{
    public IReadOnlyList<ThreadTrace> Threads { get; }

    public TraceResult(IReadOnlyList<ThreadTrace> threads)
    {
        Threads = threads ?? throw new ArgumentNullException(nameof(threads));
    }
}