using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class 
            where TImplementation : TService
        {
            builder.RegisterService<TService>(new TransientLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TService : class
        {
            builder.RegisterService<TService>(new TransientLifetime(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class
            where TImplementation : TService
        {
            return builder.RegisterService<TService>(new SingletonLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
        }

        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, TService instance)
            where TService : class
        {
            builder.RegisterTransient<TService>(sp => instance);
            return builder;
        }

        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class
            where TImplementation : TService
        {
            return builder.RegisterService<TService>(new ScopedLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
        }
        public static IServiceProviderBuilder RegisterScoped<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TService : class
        {
            return builder.RegisterService<TService>(new ScopedLifetime(serviceFactory));
        }
    }
}
