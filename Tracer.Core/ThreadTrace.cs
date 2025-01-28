namespace Tracer;
public class ThreadTrace
{
    public int ThreadId { get; }
    public TimeSpan TotalTime { get; }
    public IReadOnlyList<MethodTrace> Methods { get; }

    public ThreadTrace(int threadId, TimeSpan totalTime, IReadOnlyList<MethodTrace> methods)
    {
        ThreadId = threadId;
        TotalTime = totalTime;
        Methods = methods ?? throw new ArgumentNullException(nameof(methods));
    }
}