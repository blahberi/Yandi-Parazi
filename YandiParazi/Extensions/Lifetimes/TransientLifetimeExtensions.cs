using Yandi.Abstractions;
using Yandi.Core.LifetimeManagers;

namespace Yandi.Extensions.Lifetimes;

public static class TransientLifetimeExtensions
{
    private static readonly LifetimeManagerFactory LifetimeManagerFactory = static sf => new TransientLifetime(sf);
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(LifetimeManagerFactory, serviceFactory);
    }

    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService, TImplementation>(LifetimeManagerFactory);
    }

    public static IServiceCollection AddTransientDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        return collection.Decorate<TService, TDecorator>(LifetimeManagerFactory);
    }
}
