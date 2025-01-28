using System.Collections.ObjectModel;

namespace Tracer;

public class TraceResult
{
    public IReadOnlyList<ThreadTrace> Threads { get; }

    public TraceResult(IEnumerable<ThreadTrace> threads)
    {
        Threads = new ReadOnlyCollection<ThreadTrace>(threads.ToList());
    }
}