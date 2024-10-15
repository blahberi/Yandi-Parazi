using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new TransientLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new TransientLifetime(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new SingletonLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new SingletonLifetime(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new ScopedLifetime(
                    DefaultServiceFactory.GetServiceFactory<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new ScopedLifetime(serviceFactory));
            return builder;
        }
    }
}
