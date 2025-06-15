namespace Cornflakes.ServiceCreation;

public class ServiceFactoryBuilder<TService> : IServiceFactoryBuilder<TService>
{
    private readonly ServiceCreator<TService> serviceCreator;
    private List<ServiceInitializer> onInitialized = [];

    public ServiceFactoryBuilder(ServiceCreator<TService> serviceCreator)
    {
        this.serviceCreator = serviceCreator;
    }
    
    public IServiceFactoryBuilder<TService> Add(ServiceInitializer serviceInitializer)
    {
        this.onInitialized.Add(serviceInitializer);
        return this;
    }

    public ServiceFactory Build()
    {
        return serviceProvider => new ServiceContainer(this.serviceCreator(serviceProvider)!, this.onInitialized);
    }
}
