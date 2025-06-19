using Yandi.Extensions.Lifetimes;
using Yandi.LifetimeManagers;
using Yandi.Scopes;
using Yandi.ServiceCreation;

namespace Yandi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddService<TService>(this IServiceCollection collection, ILifetimeManager lifetimeManager)
    where TService : class
    {
        collection.Add(new ServiceDescriptor(
            typeof(TService),
            lifetimeManager
        ));

        return collection;
    }

    public static IServiceCollection AddService<TService>(this IServiceCollection collection,
    LifetimeManagerFactory lifetimeManagerFactory,
    ServiceFactory serviceFactory)
    where TService : class
    {
        ServiceDescriptor serviceDescriptor = new(
            typeof(TService),
            lifetimeManagerFactory(serviceFactory)
        );
        return collection.AddService<TService>(lifetimeManagerFactory(serviceFactory));
    }

    public static IServiceCollection AddService<TService, TImplementation>(this IServiceCollection collection,
        LifetimeManagerFactory lifetimeManagerFactory)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService>(lifetimeManagerFactory(
            DependencyResolver.GetServiceFactory<TImplementation>()
        ));
    }

    public static IServiceCollection AddServices(this IServiceCollection collection, IServiceCollection services)
    {
        foreach (ServiceDescriptor service in services)
        {
            collection.Add(service);
        }
        return collection;
    }

    public static ServiceDescriptor FindService<TService>(this IServiceCollection collection)
    {
        Type serviceType = typeof(TService);
        ServiceDescriptor? descriptor = collection.LastOrDefault(s => s.ServiceType == serviceType);

        return descriptor ?? throw new InvalidOperationException(
            $"Service of type {serviceType.FullName} not found in the collection.");
    }

    public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection collection,
        LifetimeManagerFactory lifetimeManagerFactory)
        where TService : class
        where TDecorator : class, TService
    {
        ServiceDescriptor originalDescriptor = collection.FindService<TService>();
        DecoratorFactory decoratorFactory = DependencyResolver.GetDecoratorFactory<TService, TDecorator>();
        ServiceFactory serviceFactory = sp =>
        {
            object originalInstance = originalDescriptor.LifetimeManager.GetInstance(sp);
            return decoratorFactory(sp, originalInstance);
        };
        return collection.AddService<TService>(lifetimeManagerFactory, serviceFactory);
    }

    public static IServiceProvider BuildProvider(this IServiceCollection collection)
    {
        collection
            .AddTransient<IServiceProvider>(sp => sp)
            .AddSingleton<IServiceProviderFactroy>(_ => new ServiceProviderFactory(collection))
            .AddSingleton<IScopeService, ScopeService>()
            .Finalize();
        return new ServiceProvider(collection);
    }
}