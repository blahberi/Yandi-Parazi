using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService>(new TransientLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
        {
            builder.RegisterService<TService>(new TransientLifetime(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService>(new SingletonLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
        {
            builder.RegisterService<TService>(new SingletonLifetime(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService>(new ScopedLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterScoped<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
        {
            builder.RegisterService<TService>(new ScopedLifetime(serviceFactory));
            return builder;
        }
    }
}
