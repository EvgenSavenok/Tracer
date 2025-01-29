namespace Tracer.Core.Tests;

public class TracerTests
{
    [Fact]
    public void StartStopTrace_ShouldRecordMethodExecution()
    {
        var tracer = new Tracer();

        tracer.StartTrace();
        Thread.Sleep(100);
        tracer.StopTrace();

        var result = tracer.GetTraceResult();

        Assert.Single(result.Threads);
        var threadTrace = result.Threads[0];
        Assert.Single(threadTrace.Methods);
        Assert.True(threadTrace.Methods[0].ExecutionTime >= 100);
    }
    
    [Fact]
    public void TotalExecutionTime_ShouldSumExecutionTimes()
    {
        var methods = new List<MethodTrace>();
        var method = new MethodTrace("Method1", "TestClass");
        method.Start();
        Thread.Sleep(100);
        method.Stop();
        methods.Add(method);
        
        method = new MethodTrace("Method2", "TestClass");
        method.Start();
        Thread.Sleep(200);
        method.Stop();
        methods.Add(method);

        var threadTrace = new ThreadTrace(1, methods);
        var totalExecutionTime = threadTrace.TotalExecutionTime;
        
        Assert.True(totalExecutionTime >= 300);
        Assert.Equal(1, threadTrace.ThreadId);
    }
}