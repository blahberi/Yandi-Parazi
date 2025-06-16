using Cornflakes.ServiceCreation;

namespace Cornflakes.LifetimeManagers;

internal class TransientLifetime : ILifetimeManager
{
    private readonly ServiceFactory serviceFactory;
    public TransientLifetime(ServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    public object GetInstance(IServiceProvider serviceProvider)
    {
        return this.serviceFactory(serviceProvider);
    }
}
