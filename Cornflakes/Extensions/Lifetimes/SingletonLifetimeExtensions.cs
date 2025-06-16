using Cornflakes.LifetimeManagers;
using Cornflakes.ServiceCreation;

namespace Cornflakes.Extensions;

public static class SingletonLifetimeExtensions
{
    
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(new SingletonLifetime(serviceFactory));
    }
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceCreator serviceCreator)
        where TService : class
    {
        return collection.AddSingleton<TService>(serviceCreator.ToFactory().Build());
    }
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new SingletonLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>()
        ));
    }
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService instance)
        where TService : class
    {
        return collection.AddTransient<TService>(sp => instance);
    }

}