using Cornflakes.LifetimeManagers;
using Cornflakes.Scopes;

namespace Cornflakes.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddService<TService>(this IServiceCollection collection, ILifetimeManager lifetimeManager)
    {
        collection.Add(new ServiceDescriptor(
            typeof(TService), 
            lifetimeManager
        ));

        return collection;
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

    public static IServiceProvider BuildServiceProvider(this IServiceCollection collection)
    {
        collection
            .AddTransient<IServiceProvider>(sp => sp)
            .AddSingleton<IServiceProviderFactroy>(_ => new ServiceProviderFactory(collection))
            .AddSingleton<IScopeService, ScopeService>()
            .Finalize();
        return new ServiceProvider(collection);
    }
}