using Cornflakes.CreationStrategies;

namespace Cornflakes
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new TransientCreator(
                    (IServiceProvider servicePorvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), servicePorvider)
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new TransientCreator(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new SingletonCreation(
                    (IServiceProvider serviceProvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), serviceProvider)
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new SingletonCreation(serviceFactory));
            return builder;
        }

        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new ScopedCreation(
                    (IServiceProvider serviceProvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), serviceProvider)
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TImplementation : TService
        {
            builder.RegisterService<TService, TImplementation>(new ScopedCreation(serviceFactory));
            return builder;
        }
    }
}
