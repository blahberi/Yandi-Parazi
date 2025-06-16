using System.Collections.Concurrent;
using Cornflakes.Extensions;
using Cornflakes.Scopes;
using Cornflakes.ServiceCreation;

namespace Cornflakes.LifetimeManagers;

internal class ScopedLifetime : ILifetimeManager
{
    private readonly ConcurrentDictionary<IScope, object> instances;
    private readonly ServiceFactory serviceFactory;

    public ScopedLifetime(ServiceFactory serviceFactory)
    {
        this.serviceFactory = serviceFactory;
        this.instances = new ConcurrentDictionary<IScope, object>();
    }

    public object GetInstance(IServiceProvider serviceProvider)
    {
        return this.instances.GetOrAdd(serviceProvider.GetScope(), _ =>
        {
            object instance = this.serviceFactory(serviceProvider);
            serviceProvider.GetScope().Subscribe(this.scopedDisposed);
            return instance;
        });
    }

    private void scopedDisposed(IScope scope)
    {
        this.instances.Remove(scope, out _);
    }
}
