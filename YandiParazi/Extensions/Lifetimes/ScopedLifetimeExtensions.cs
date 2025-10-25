using Yandi.Abstractions;
using Yandi.Core.LifetimeManagers;

namespace Yandi.Extensions.Lifetimes;

public static class ScopedLifetimeExtensions
{
    private static readonly LifetimeManagerFactory Scoped = static sf => new ScopedLifetime(sf);
    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(Scoped, serviceFactory);
    }

    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService, TImplementation>(Scoped);
    }

    public static IServiceCollection AddScopedDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        return collection.Decorate<TService, TDecorator>(Scoped);
    }
}
