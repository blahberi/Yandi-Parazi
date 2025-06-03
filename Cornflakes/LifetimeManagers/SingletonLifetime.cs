namespace Cornflakes.LifetimeManagers;

internal class SingletonLifetime : ILifetimeManager
{
    private IServiceContainer? container;
    private readonly IServiceFactory serviceFactory;
    private readonly object lockObject = new object();

    public SingletonLifetime(IServiceFactory serviceFactory)
    {
        this.serviceFactory = serviceFactory;
    }

    private bool Initialized => this.container != null;

    public void Initialize(IServiceProvider serviceProvider)
    {
        if (this.Initialized) return;
        lock (this.lockObject)
        {
            if (this.Initialized) return;
            this.container = this.serviceFactory.Create(serviceProvider);
        }
    }

    public object GetInstance(IServiceProvider serviceProvider)
    {
        if (this.Initialized) return this.container!.GetService(serviceProvider);
        lock (this.lockObject)
        {
            if (!this.Initialized) this.Initialize(serviceProvider);
            return this.container!.GetService(serviceProvider);
        }
    }
}
