using System.Collections.Concurrent;
using System.Diagnostics;
using Tracer.Coree.Contracts;

namespace Tracer;

public class Tracer : ITracer
{
    private readonly ConcurrentDictionary<int, Stack<MethodTrace>> _methodStacks;
    private readonly ConcurrentDictionary<int, List<MethodTrace>> _threadMethods;

    public Tracer()
    {
        _methodStacks = new ConcurrentDictionary<int, Stack<MethodTrace>>();
        _threadMethods = new ConcurrentDictionary<int, List<MethodTrace>>();
    }
    
    public void StartTrace()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        var stack = _methodStacks.GetOrAdd(threadId, _ => new Stack<MethodTrace>());

        var method = new StackTrace().GetFrame(1)?.GetMethod();
        if (method == null) 
            return;

        var measuredMethod = new MethodTrace(method.Name, method.DeclaringType?.Name!);
        measuredMethod.Start();

        if (stack.Count > 0)
        {
            MethodTrace outerMethod = stack.Peek();
            outerMethod.Methods.Add(measuredMethod);
        }
        else
        {
            var methods = _threadMethods.GetOrAdd(threadId, _ => new List<MethodTrace>());
            methods.Add(measuredMethod);
        }
        stack.Push(measuredMethod);
    }

    public void StopTrace()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        if (_methodStacks.TryGetValue(threadId, out var stack) && stack.Count > 0)
        {
            var methodTrace = stack.Pop();
            methodTrace.Stop();
        }
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult(_threadMethods.Select(pair => new ThreadTrace(pair.Key, pair.Value)));
    }
}