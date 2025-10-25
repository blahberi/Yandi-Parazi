using Yandi.Abstractions;

namespace Yandi.Core.LifetimeManagers;

internal class SingleInstanceLifetime(ServiceFactory serviceFactory) : ILifetimeManager
{
    private object? instance;
    private readonly ServiceFactory serviceFactory = serviceFactory;
    private readonly Lock lockObject = new();

    private bool Initialized => this.instance != null;

    public void Initialize(IServiceProvider serviceProvider)
    {
        if (this.Initialized) return;
        lock (this.lockObject)
        {
            if (this.Initialized) return;
            this.instance = this.serviceFactory(serviceProvider);
        }
    }

    public object GetInstance(IServiceProvider serviceProvider)
    {
        if (this.Initialized) return this.instance!;
        lock (this.lockObject)
        {
            if (!this.Initialized) this.Initialize(serviceProvider);
            return this.instance!;
        }
    }
}
