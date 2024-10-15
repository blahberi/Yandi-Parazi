using Cornflakes.LifetimeStrategies;

namespace Cornflakes.Extensions
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new TransientLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return collection;
        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new TransientLifetime(serviceFactory));
            return collection;
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new SingletonLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return collection;
        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new SingletonLifetime(serviceFactory));
            return collection;
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new ScopedLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return collection;
        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new ScopedLifetime(serviceFactory));
            return collection;
        }
    }
}
