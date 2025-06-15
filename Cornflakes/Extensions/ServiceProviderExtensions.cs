using Cornflakes.Scopes;

namespace Cornflakes.Extensions;

public static class ServiceProviderExtensions
{
    public static TService? GetService<TService>(this IServiceProvider serviceProvider)
    {
        return (TService?)serviceProvider.GetService(typeof(TService));
    }
    public static bool TryGetService<TService>(this IServiceProvider serviceProvider, out TService? service)
    {
        service = serviceProvider.GetService<TService>();
        return service != null;
    }

    public static TService MustGetService<TService>(this IServiceProvider serviceProvider)
    {
        if (serviceProvider.TryGetService(out TService? service))
        {
            return service!;
        }
        throw new InvalidOperationException($"Service of type {typeof(TService)} not found.");
    }

    public static IScope CreateScope(this IServiceProvider serviceProvider)
    {
        return serviceProvider.MustGetService<IScopeService>().CreateScope();
    }

    public static IScope GetScope(this IServiceProvider scopedProvider)
    {
        return scopedProvider.MustGetService<IScopeService>().GetScope(scopedProvider);
    }
}