namespace Cornflakes;

public class ServiceFactoryBuilder<TService> : IServiceFactoryBuilder<TService>
{
    private ServiceCreator<TService> serviceCreator;
    private List<ServiceInitializer> onInitialized = [];

    public ServiceFactoryBuilder(ServiceCreator<TService> serviceCreator)
    {
        this.serviceCreator = serviceCreator;
    }
    
    public IServiceFactoryBuilder<TService> Add(ServiceCreationWrapper<TService> serviceProviderWrapper)
    {
        ServiceCreator<TService> currentCreator = this.serviceCreator;
        this.serviceCreator = serviceProvider => 
            serviceProviderWrapper(serviceProvider, currentCreator(serviceProvider));
        return this;
    }

    public IServiceFactoryBuilder<TService> Add(ServiceInitializer serviceInitializer)
    {
        this.onInitialized.Add(serviceInitializer);
        return this;
    }

    public IServiceFactory Build()
    {
        return new ServiceFactory(this.serviceCreator.ToGenrealCreator(), this.onInitialized);
    }
}

internal static class ServiceCreatorExtensions
{
    public static ServiceCreator<object> ToGenrealCreator<TService>(this ServiceCreator<TService> creator)
    {
        return serviceProvider => creator(serviceProvider)!;
    }
}
