using Cornflakes.ServiceCreation;

namespace Cornflakes.LifetimeManagers;

internal class SingletonLifetime : ILifetimeManager
{
    private IServiceContainer? container;
    private readonly ServiceFactory serviceFactory;
    private readonly Lock lockObject = new();

    public SingletonLifetime(ServiceFactory serviceFactory)
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
            this.container = this.serviceFactory(serviceProvider);
        }
    }

    public IServiceContainer GetInstance(IServiceProvider serviceProvider)
    {
        if (this.Initialized) return this.container!;
        lock (this.lockObject)
        {
            if (!this.Initialized) this.Initialize(serviceProvider);
            return this.container!;
        }
    }
}
