namespace Cornflakes.ServiceCreation;

internal class ServiceFactory : IServiceFactory
{
    private ServiceCreator<object> serviceCreator;
    private List<ServiceInitializer> onInitialized;
    
    public ServiceFactory(ServiceCreator<object> serviceCreator, List<ServiceInitializer> onInitialized)
    {
        this.serviceCreator = serviceCreator;
        this.onInitialized = onInitialized;
    }
    
    public IServiceContainer Create(IServiceProvider serviceProvider)
    {
        object instance = this.serviceCreator(serviceProvider);
        return new ServiceContainer(instance, this.onInitialized);
    }
}