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
                    DependencyResolver.GetServiceFactory<TImplementation>().UseMemberInjection<TImplementation>()
                ));
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection,
            OnInitialized onInitialized)
        {
            return collection.AddService<TService>(new TransientLifetime(onInitialized));
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddTransient<TService>(serviceFactory.ToPipeline());
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : TService
        {
            return collection.AddService<TService>(new SingletonLifetime(
                    DependencyResolver.GetServiceFactory<TImplementation>().UseMemberInjection<TImplementation>()
                ));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, OnInitialized onInitialized)
            where TService : class
        {
            return collection.AddService<TService>(new SingletonLifetime(onInitialized));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddSingleton<TService>(serviceFactory.ToPipeline());
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
                    DependencyResolver.GetServiceFactory<TImplementation>().UseMemberInjection<TImplementation>()
                ));
        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection, OnInitialized onInitialized)
            where TService : class
        {
            return collection.AddService<TService>(new ScopedLifetime(onInitialized));
        }

        public static IServiceCollection AddScoped<TService>(this IServiceCollection collection,
            ServiceFactory serviceFactory)
            where TService : class
        {
            return collection.AddScoped<TService>(serviceFactory.ToPipeline());
        }
    }
}
