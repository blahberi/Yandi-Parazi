using Cornflakes.LifetimeManagers;

namespace Cornflakes.Extensions
{
    public static class ServiceProviderBuilderExtensions
    {
        public static IServiceProviderBuilder RegisterTransient<TService>(this IServiceProviderBuilder builder, IServiceFactory serviceFactory)
            where TService : class
        {
            builder.RegisterService<TService>(new TransientLifetime(serviceFactory));
            return builder;
        }
        public static IServiceProviderBuilder RegisterTransient<TService>(this IServiceProviderBuilder builder, ServiceCreator<TService> serviceCreator)
            where TService : class
        {
            return builder.RegisterTransient<TService>(serviceCreator.ToFactory().Build());
        }
        public static IServiceProviderBuilder RegisterTransient<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class 
            where TImplementation : TService
        {
            builder.RegisterService<TService>(new TransientLifetime(
                    DependencyResolver.GetServiceFactory<TService, TImplementation>().WithMemberInjection<TService, TImplementation>().Build()
                ));
            return builder;
        }

        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, IServiceFactory creationPipeline)
            where TService : class
        {
            return builder.RegisterService<TService>(new SingletonLifetime(creationPipeline));
        }
        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, ServiceCreator<TService> serviceCreator)
            where TService : class
        {
            return builder.RegisterSingleton<TService>(serviceCreator.ToFactory().Build());
        }
        public static IServiceProviderBuilder RegisterSingleton<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class
            where TImplementation : TService
        {
            return builder.RegisterService<TService>(new SingletonLifetime(
                    DependencyResolver.GetServiceFactory<TService, TImplementation>().WithMemberInjection<TService, TImplementation>().Build()
                ));
        }
        public static IServiceProviderBuilder RegisterSingleton<TService>(this IServiceProviderBuilder builder, TService instance)
            where TService : class
        {
            builder.RegisterTransient<TService>(sp => instance);
            return builder;
        }

        public static IServiceProviderBuilder RegisterScoped<TService>(this IServiceProviderBuilder builder, IServiceFactory serviceFactory)
            where TService : class
        {
            return builder.RegisterService<TService>(new ScopedLifetime(serviceFactory));
        }
        public static IServiceProviderBuilder RegisterScoped<TService>(this IServiceProviderBuilder builder,
            ServiceCreator<TService> serviceCreator)
            where TService : class
        {
            return builder.RegisterScoped<TService>(serviceCreator.ToFactory().Build());
        }
        public static IServiceProviderBuilder RegisterScoped<TService, TImplementation>(this IServiceProviderBuilder builder)
            where TService : class
            where TImplementation : TService
        {
            return builder.RegisterService<TService>(new ScopedLifetime(
                    DependencyResolver.GetServiceFactory<TService, TImplementation>().WithMemberInjection<TService, TImplementation>().Build()
                ));
        }
    }
}
