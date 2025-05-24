using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public static class ServiceCollectionExtenions
    {
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new TransientLifetime(
                    DependencyResolver.GetServiceFactory<TImplementation>().AttachMemberInjection<TImplementation>()
                ));
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection,
            ServiceLoader serviceLoader)
        {
            return collection.AddService<TService>(new TransientLifetime(serviceLoader));
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddTransient<TService>(serviceFactory.ToLoader());
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new SingletonLifetime(
                    DependencyResolver.GetServiceFactory<TImplementation>().AttachMemberInjection<TImplementation>()
                ));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceLoader serviceLoader)
            where TService : class
        {
            return collection.AddService<TService>(new SingletonLifetime(serviceLoader));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddSingleton<TService>(serviceFactory.ToLoader());
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, TService instance)
            where TService : class
        {
            return collection.AddTransient<TService>(sp => instance);
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new ScopedLifetime(
                    DependencyResolver.GetServiceFactory<TImplementation>().AttachMemberInjection<TImplementation>()
                ));
        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, ServiceLoader serviceLoader)
            where TService : class
        {
            return collection.AddService<TService>(new ScopedLifetime(serviceLoader));
        }

        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection,
            ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddScoped<TService>(serviceFactory.ToLoader());
        }
    }
}
