using Cornflakes.LifetimeManagers;

namespace Cornflakes.Extensions;
public static class ServiceCollectionExtenions
{
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, IServiceCreationPipeline creationPipeline)
    {
        return collection.AddService<TService>(new TransientLifetime(creationPipeline));
    }
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddTransient<TService>(serviceFactory.ToPipeline().Build());
    }
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new TransientLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>().WithMemberInjection<TImplementation>().Build()
        ));
    }
        
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, IServiceCreationPipeline creationPipeline)
        where TService : class
    {
        return collection.AddService<TService>(new SingletonLifetime(creationPipeline));
    }
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddSingleton<TService>(serviceFactory.ToPipeline().Build());
    }
    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new SingletonLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>().WithMemberInjection<TImplementation>().Build()
        ));
    }
    public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService instance)
        where TService : class
    {
        return collection.AddTransient<TService>(sp => instance);
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, IServiceCreationPipeline creationPipeline)
        where TService : class
    {
        return collection.AddService<TService>(new ScopedLifetime(creationPipeline));
    }
    public static IServiceCollection AddScoped<TService>(this IServiceCollection collection,
        ServiceFactory serviceFactory)
        where TService : class
    {
        return collection.AddScoped<TService>(serviceFactory.ToPipeline().Build());
    }
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new ScopedLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>().WithMemberInjection<TImplementation>().Build()
        ));
    }
}