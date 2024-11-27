using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            collection.AddService<TService>(new TransientLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return collection;
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            collection.AddService<TService>(new TransientLifetime(serviceFactory));
            return collection;
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            collection.AddService<TService>(new SingletonLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return collection;
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            collection.AddService<TService>(new SingletonLifetime(serviceFactory));
            return collection;
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService instance)
            where TService : class
        {
            collection.AddTransient<TService>(sp => instance);
            return collection;
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            collection.AddService<TService>(new ScopedLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return collection;
        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            collection.AddService<TService>(new ScopedLifetime(serviceFactory));
            return collection;
        }
    }
}
