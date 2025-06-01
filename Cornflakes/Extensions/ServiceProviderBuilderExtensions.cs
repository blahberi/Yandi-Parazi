using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class 
            where TImplementation : TService
        {
            builder.RegisterService<TService>(new TransientLifetime(
                    DependencyResolver.GetServiceFactory<TImplementation>().UseMemberInjection<TImplementation>()
                ));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService>(this IServiceProviderBuilder builder, ServiceCreationPipeline serviceCreationPipeline)
            where TService : class
        {
            builder.RegisterService<TService>(new TransientLifetime(serviceCreationPipeline));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TService : class
        {
            return builder.RegisterTransient<TService>(serviceFactory.ToPipeline());
        }

        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class
            where TImplementation : TService
        {
            return builder.RegisterService<TService>(new SingletonLifetime(
                    DependencyResolver.GetServiceFactory<TImplementation>().UseMemberInjection<TImplementation>()
                ));
        }
        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, ServiceCreationPipeline serviceCreationPipeline)
            where TService : class
        {
            return builder.RegisterService<TService>(new SingletonLifetime(serviceCreationPipeline));
        }
        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
            where TService : class
        {
            return builder.RegisterSingleton<TService>(serviceFactory.ToPipeline());
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
                    DependencyResolver.GetServiceFactory<TImplementation>().UseMemberInjection<TImplementation>()
                ));
        }
        public static IServiceProviderBuilder RegisterScoped<TService>(this IServiceProviderBuilder builder, ServiceCreationPipeline serviceCreationPipeline)
            where TService : class
        {
            return builder.RegisterService<TService>(new ScopedLifetime(serviceCreationPipeline));
        }

        public static IServiceProviderBuilder RegisterScoped<TService>(this IServiceProviderBuilder builder,
            ServiceFactory serviceFactory)
            where TService : class
        {
            return builder.RegisterScoped<TService>(serviceFactory.ToPipeline());
        }
    }
}
