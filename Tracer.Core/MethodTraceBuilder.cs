using System.Diagnostics;

namespace Tracer;

public class MethodTraceBuilder
{
    private readonly string _methodName;
    private readonly string _className;
    private readonly Stopwatch _stopwatch;
    private readonly List<MethodTrace> _innerMethods;

    public MethodTraceBuilder(string methodName, string className)
    {
        _methodName = methodName;
        _className = className;
        _stopwatch = new Stopwatch();
        _innerMethods = new List<MethodTrace>();
    }

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop()
    {
        _stopwatch.Stop();
    }

    public void AddInnerMethod(MethodTraceBuilder innerMethodBuilder)
    {
        _innerMethods.Add(innerMethodBuilder.BuildMethodTrace());
    }

    public MethodTrace BuildMethodTrace()
    {
        return new MethodTrace(_methodName, _className, _stopwatch.Elapsed, _innerMethods);
    }
}