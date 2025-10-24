using Yandi.Abstractions;

namespace Yandi.Core.LifetimeManagers;

internal class TransientLifetime(ServiceFactory serviceFactory) : ILifetimeManager
{
    private readonly ServiceFactory serviceFactory = serviceFactory;

    public object GetInstance(IServiceProvider serviceProvider)
    {
        return this.serviceFactory(serviceProvider);
    }
}
