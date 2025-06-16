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
    
    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new ScopedLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>()
        ));
    }
    
    public static IServiceCollection AddScopedDecorator<TService, TDecorator>(this IServiceCollection collection)
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