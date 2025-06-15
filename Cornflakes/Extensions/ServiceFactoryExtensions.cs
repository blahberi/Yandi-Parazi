namespace Cornflakes.Extensions;

public static class ServiceFactoryExtensions
{
    public static IServiceFactoryBuilder<TService> ToFactory<TService>(this ServiceCreator<TService> serviceCreator)
    {
        return new ServiceFactoryBuilder<TService>(serviceCreator);
    }
    
    public static IServiceFactoryBuilder<TService> WithMemberInjection<TService, TImplementation>(this IServiceFactoryBuilder<TService> factoryBuilder)
        where TService : class
        where TImplementation : TService
    {
        ServiceInitializer? injector = DependencyResolver.TryGetMemberInjector<TImplementation>();
        return injector == null ? factoryBuilder : factoryBuilder.Add(injector);
    }

    public static IServiceFactory WithMemberInjection<TService, TImplementation>(this ServiceCreator<TService> serviceCreator)
        where TService : class
        where TImplementation : TService
    {
        return serviceCreator.ToFactory().WithMemberInjection<TService, TImplementation>().Build();
    }
}