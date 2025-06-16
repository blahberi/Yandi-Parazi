using Cornflakes.LifetimeManagers;
using Cornflakes.ServiceCreation;

namespace Cornflakes.Extensions;

public static class ScopedLifetimeExtensions
{
    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(new ScopedLifetime(serviceFactory));
    }
    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection,
        ServiceCreator serviceCreator)
        where TService : class
    {
        return collection.AddScoped<TService>(serviceCreator.ToFactory().Build());
    }
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new ScopedLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>()
        ));
    }
}