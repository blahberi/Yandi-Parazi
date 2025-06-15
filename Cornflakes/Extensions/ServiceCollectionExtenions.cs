using Cornflakes.LifetimeManagers;

namespace Cornflakes.Extensions;
public static class ServiceCollectionExtenions
{
    public static IServiceCollection AddService<TService>(this IServiceCollection collection, ILifetimeManager lifetimeManager)
    {
        collection.Add(new ServiceDescriptor(
            typeof(TService), 
            lifetimeManager
        ));

        return collection;
    }
    
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, IServiceFactory serviceFactory)
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
            DependencyResolver.GetServiceFactory<TService, TImplementation>().WithMemberInjection<TService, TImplementation>().Build()
        ));
    }
        
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, IServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(new SingletonLifetime(serviceFactory));
    }
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceCreator<TService> serviceCreator)
        where TService : class
    {
        return collection.AddSingleton<TService>(serviceCreator.ToFactory().Build());
    }
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new SingletonLifetime(
            DependencyResolver.GetServiceFactory<TService, TImplementation>().WithMemberInjection<TService, TImplementation>().Build()
        ));
    }
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService instance)
        where TService : class
    {
        return collection.AddTransient<TService>(sp => instance);
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, IServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddService<TService>(new ScopedLifetime(serviceFactory));
    }
    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection,
        ServiceCreator<TService> serviceCreator)
        where TService : class
    {
        return collection.AddScoped<TService>(serviceCreator.ToFactory().Build());
    }
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new ScopedLifetime(
            DependencyResolver.GetServiceFactory<TService, TImplementation>().WithMemberInjection<TService, TImplementation>().Build()
        ));
    }
}