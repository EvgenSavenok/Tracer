using System.Diagnostics;

namespace Tracer;

public class ThreadTraceBuilder
{
    private readonly int _threadId;
    private readonly Stack<MethodTraceBuilder> _methodStack;
    private readonly List<MethodTrace> _methods;

    public ThreadTraceBuilder(int threadId)
    {
        _threadId = threadId;
        _methodStack = new Stack<MethodTraceBuilder>();
        _methods = new List<MethodTrace>();
    }

    public void StartMethodTrace()
    {
        var method = new StackFrame(1).GetMethod();
        if (method == null) return;

        var methodTraceBuilder = new MethodTraceBuilder(method.Name, method.DeclaringType?.Name ?? "Unknown");
        if (_methodStack.Any())
        {
            _methodStack.Peek().AddInnerMethod(methodTraceBuilder);
        }
        _methodStack.Push(methodTraceBuilder);
        methodTraceBuilder.Start();
    }

    public void StopMethodTrace()
    {
        if (!_methodStack.Any()) return;

        var methodTraceBuilder = _methodStack.Pop();
        methodTraceBuilder.Stop();

        if (!_methodStack.Any())
        {
            _methods.Add(methodTraceBuilder.BuildMethodTrace());
        }
    }

    public ThreadTrace BuildThreadTrace()
    {
        return new ThreadTrace(_threadId, TimeSpan.FromMilliseconds(_methods.Sum(m => m.ExecutionTime.TotalMilliseconds)), _methods);
    }

}