namespace Cornflakes.ServiceCreation;

public class ServiceFactoryBuilder : IServiceFactoryBuilder
{
    private readonly ServiceCreator serviceCreator;
    private List<ServiceInitializer> onInitialized = [];

    public ServiceFactoryBuilder(ServiceCreator serviceCreator)
    {
        this.serviceCreator = serviceCreator;
    }
    
    public IServiceFactoryBuilder Add(ServiceInitializer serviceInitializer)
    {
        this.onInitialized.Add(serviceInitializer);
        return this;
    }

    public ServiceFactory Build()
    {
        return serviceProvider => new ServiceContainer(this.serviceCreator(serviceProvider)!, this.onInitialized);
    }
}
