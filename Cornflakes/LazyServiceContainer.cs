namespace Cornflakes;

public class LazyServiceContainer : IServiceContainer
{
    private object instance;
    private readonly List<ServiceInitializer> onInitialized;
    private bool initialized;

    public LazyServiceContainer(object instance, List<ServiceInitializer> onInitialized)
    {
        this.instance = instance;
        this.onInitialized = onInitialized;
    }

    public object GetService(IServiceProvider serviceProvider)
    {
        if (!this.initialized)
        {
            this.Initialize(serviceProvider);
        }
        return this.instance;
    }
    
    private void Initialize(IServiceProvider serviceProvider)
    {
        this.initialized = true;
        foreach (ServiceInitializer onInit in this.onInitialized)
        {
            onInit(serviceProvider, this.instance);
        }
    }
}