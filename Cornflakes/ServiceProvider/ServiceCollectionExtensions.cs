using Cornflakes.Extensions;
using Cornflakes.LifetimeManagers;
using Cornflakes.Scopes;

namespace Cornflakes;

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

}