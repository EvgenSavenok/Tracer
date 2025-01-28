namespace Tracer;

public class MethodTrace
{
    public string MethodName { get; }
    public string ClassName { get; }
    public TimeSpan ExecutionTime { get; }
    public IReadOnlyList<MethodTrace> InnerMethods { get; }

    public MethodTrace(string methodName, string className, TimeSpan executionTime, IReadOnlyList<MethodTrace> innerMethods)
    {
        MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
        ClassName = className ?? throw new ArgumentNullException(nameof(className));
        ExecutionTime = executionTime;
        InnerMethods = innerMethods ?? throw new ArgumentNullException(nameof(innerMethods));
    }
}