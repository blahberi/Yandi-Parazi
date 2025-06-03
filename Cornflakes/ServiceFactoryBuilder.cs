namespace Cornflakes;

public class ServiceFactoryBuilder : IServiceFactoryBuilder
{
    private ServiceCreator serviceCreator;
    private List<ServiceInitializer> onInitialized = [];

    public ServiceFactoryBuilder(ServiceCreator serviceCreator)
    {
        this.serviceCreator = serviceCreator;
    }
    
    public IServiceFactoryBuilder Add(ServiceCreationWrapper serviceProviderWrapper)
    {
        ServiceCreator currentCreator = this.serviceCreator;
        this.serviceCreator = serviceProvider => 
            serviceProviderWrapper(serviceProvider, currentCreator(serviceProvider));
        return this;
    }

    public IServiceFactoryBuilder Add(ServiceInitializer serviceInitializer)
    {
        this.onInitialized.Add(serviceInitializer);
        return this;
    }

    public IServiceFactory Build()
    {
        return new ServiceFactory(this.serviceCreator, this.onInitialized);
    }
}