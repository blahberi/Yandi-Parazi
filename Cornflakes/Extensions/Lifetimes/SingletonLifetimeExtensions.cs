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
    
    public static IServiceCollection AddSingletonDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        ServiceDescriptor originalDescriptor = collection.FindService<TService>();
        DecoratorFactory decoratorFactory = DependencyResolver.GetDecoratorFactory<TService, TDecorator>();
        ILifetimeManager decoratorLifetime = new SingletonLifetime(sp =>
        {
            object originalInstance = originalDescriptor.LifetimeManager.GetInstance(sp);
            return decoratorFactory(sp, originalInstance);
        });
        return collection.AddService<TService>(decoratorLifetime);
    }
}