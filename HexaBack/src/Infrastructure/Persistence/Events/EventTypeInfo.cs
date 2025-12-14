using System.Reflection;

namespace Persistence.Events;

public readonly struct EventTypeInfo
{
    public Type HandlerType { get; init; }
    public MethodInfo HandleMethod { get; init; }
    public Func<IServiceProvider, object[]> HandlersFactory { get; init; }
}
