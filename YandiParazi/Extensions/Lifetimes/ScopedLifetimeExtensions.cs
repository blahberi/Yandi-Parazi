using Yandi.LifetimeManagers;
using Yandi.ServiceCreation;

namespace Yandi.Extensions;

public static class ScopedLifetimeExtensions
{
    private static readonly LifetimeManagerFactory LifetimeManagerFactory = sf => new ScopedLifetime(sf);
    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(new ScopedLifetime(serviceFactory));
    }
    
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService, TImplementation>(LifetimeManagerFactory);
    }
    
    public static IServiceCollection AddScopedDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        return collection.Decorate<TService, TDecorator>(sf => new SingletonLifetime(sf));
    }
}