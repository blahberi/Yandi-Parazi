using Cornflakes.ServiceCreation;

namespace Cornflakes.Extensions;

public static class ServiceFactoryExtensions
{
    public static IServiceFactoryBuilder ToFactory(this ServiceCreator serviceCreator)
    {
        return new ServiceFactoryBuilder(serviceCreator);
    }
    
    public static IServiceFactoryBuilder WithMemberInjection<TImplementation>(this IServiceFactoryBuilder factoryBuilder)
    {
        ServiceInitializer? injector = DependencyResolver.TryGetMemberInjector<TImplementation>();
        return injector == null ? factoryBuilder : factoryBuilder.Add(injector);
    }

    public static ServiceFactory WithMemberInjection<TImplementation>(this ServiceCreator serviceCreator)
    {
        return serviceCreator.ToFactory().WithMemberInjection<TImplementation>().Build();
    }
}