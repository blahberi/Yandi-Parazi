using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new TransientLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddService<TService>(new TransientLifetime(serviceFactory));
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new SingletonLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddService<TService>(new SingletonLifetime(serviceFactory));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService instance)
            where TService : class
        {
            return collection.AddSingleton<TService>(sp => instance);
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new ScopedLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddService<TService>(new ScopedLifetime(serviceFactory));
        }
    }
}
