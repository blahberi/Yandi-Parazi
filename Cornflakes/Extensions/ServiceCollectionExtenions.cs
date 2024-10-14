using Cornflakes.CreationStrategies;

namespace Cornflakes.Extensions
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new TransientCreator(
                    (IServiceProvider servicePorvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), servicePorvider)
                ));
            return collection;
        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new TransientCreator(serviceFactory));
            return collection;
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new SingletonCreation(
                    (IServiceProvider serviceProvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), serviceProvider)
                ));
            return collection;
        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new SingletonCreation(serviceFactory));
            return collection;
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new ScopedCreation(
                    (IServiceProvider serviceProvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), serviceProvider)
                ));
            return collection;
        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            collection.AddService<TService, TImplementation>(new ScopedCreation(serviceFactory));
            return collection;
        }
    }
}
