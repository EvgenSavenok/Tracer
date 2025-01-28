using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tracer.Contracts;

namespace Tracer;

public class Tracer : ITracer
{
    private readonly ConcurrentDictionary<int, ThreadTraceBuilder> _threadTraces = new();

    public void StartTrace()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        var threadTraceBuilder = _threadTraces.GetOrAdd(threadId, id => new ThreadTraceBuilder(id));
        threadTraceBuilder.StartMethodTrace();
    }

    public void StopTrace()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        if (_threadTraces.TryGetValue(threadId, out var threadTraceBuilder))
        {
            threadTraceBuilder.StopMethodTrace();
        }
    }

    public TraceResult GetTraceResult()
    {
        var threadTraces = _threadTraces.Values.Select(builder => builder.BuildThreadTrace()).ToList();
        return new TraceResult(threadTraces);
    }
}
