using Cornflakes.LifetimeManagers;
using Cornflakes.ServiceCreation;

namespace Cornflakes.Extensions;

public static class TransientLifetimeExtensions
{
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
    {
        return collection.AddService<TService>(new TransientLifetime(serviceFactory));
    }
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceCreator<TService> serviceCreator)
        where TService : class
    {
        return collection.AddTransient<TService>(serviceCreator.ToFactory().Build());
    }
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new TransientLifetime(
            DependencyResolver.GetServiceFactory<TService, TImplementation>()
        ));
    }
}