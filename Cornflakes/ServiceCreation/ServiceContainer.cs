namespace Cornflakes.ServiceCreation;

internal class ServiceContainer : IServiceContainer
{
    private readonly object instance;
    private readonly List<ServiceInitializer> onInitialized;
    private bool initialized;

    public ServiceContainer(object instance, List<ServiceInitializer> onInitialized)
    {
        this.instance = instance;
        this.onInitialized = onInitialized;
        this.initialized = false;
    }
    
    public object GetService(IServiceProvider serviceProvider)
    {
        if (!this.initialized)
        {
            this.InitializeInstance(serviceProvider);
        }

        return this.instance;
    }
    
    private void InitializeInstance(IServiceProvider serviceProvider)
    {
        if (this.initialized)
        {
            return;
        }
        
        this.initialized = true;
        
        foreach (ServiceInitializer initializer in this.onInitialized)
        {
            initializer(serviceProvider, this.instance);
        }
    }
}