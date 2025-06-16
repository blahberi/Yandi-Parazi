using Cornflakes.LifetimeManagers;
using Cornflakes.ServiceCreation;

namespace Cornflakes.Extensions;

public static class TransientLifetimeExtensions
{
    public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
    {
        return collection.AddService<TService>(new TransientLifetime(serviceFactory));
    }
    
    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : TService
    {
        return collection.AddService<TService>(new TransientLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>()
        ));
    }

    public static IServiceCollection AddTransientDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        ServiceDescriptor originalDescriptor = collection.FindService<TService>();
        DecoratorFactory decoratorFactory = DependencyResolver.GetDecoratorFactory<TService, TDecorator>();
        ILifetimeManager decoratorLifetime = new TransientLifetime(sp =>
        {
            object originalInstance = originalDescriptor.LifetimeManager.GetInstance(sp);
            return decoratorFactory(sp, originalInstance);
        });
        return collection.AddService<TService>(decoratorLifetime);
    }
}