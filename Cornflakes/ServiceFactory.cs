namespace Cornflakes;

internal class ServiceFactory : IServiceFactory
{
    private ServiceCreator serviceCreator;
    private List<ServiceInitializer> onInitialized;
    
    public ServiceFactory(ServiceCreator serviceCreator, List<ServiceInitializer> onInitialized)
    {
        this.serviceCreator = serviceCreator;
        this.onInitialized = onInitialized;
    }
    
    public IServiceContainer Create(IServiceProvider serviceProvider)
    {
        object instance = this.serviceCreator(serviceProvider);
        return new LazyServiceContainer(instance, this.onInitialized);
    }
}