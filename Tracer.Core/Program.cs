using Tracer.Contracts;

namespace Tracer;

class Program
{
    private static ITracer _tracer = new Tracer();

    static void Main()
    {
        _tracer.StartTrace();
        Method1();
        _tracer.StopTrace();

        var thread = new Thread(Method2);
        thread.Start();
        thread.Join();

        var traceResult = _tracer.GetTraceResult();

        PrintTraceResult(traceResult);
    }

    static void Method1()
    {
        _tracer.StartTrace();
        Thread.Sleep(100); 
        Method1Inner();
        _tracer.StopTrace();
    }

    static void Method1Inner()
    {
        _tracer.StartTrace();
        Thread.Sleep(50); 
        _tracer.StopTrace();
    }

    static void Method2()
    {
        _tracer.StartTrace();
        Thread.Sleep(200); 
        _tracer.StopTrace();
    }

    static void PrintTraceResult(TraceResult traceResult)
    {
        foreach (var threadTrace in traceResult.Threads)
        {
            Console.WriteLine($"Thread {threadTrace.ThreadId}: Total Time = {threadTrace.TotalTime.TotalMilliseconds}ms");
            foreach (var method in threadTrace.Methods)
            {
                PrintMethodTrace(method, 1);
            }
        }
    }

    static void PrintMethodTrace(MethodTrace method, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 4);
        Console.WriteLine($"{indent}Method {method.MethodName} in class {method.ClassName} executed in {method.ExecutionTime.TotalMilliseconds}ms");

        foreach (var innerMethod in method.InnerMethods)
        {
            PrintMethodTrace(innerMethod, indentLevel + 1);
        }
    }
}