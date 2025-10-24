using System.Collections.Concurrent;
using Yandi.Abstractions;
using Yandi.Extensions;

namespace Yandi.Core.LifetimeManagers;

internal class ScopedLifetime(ServiceFactory serviceFactory) : ILifetimeManager
{
    private readonly ConcurrentDictionary<IScope, object> instances = new();
    private readonly ServiceFactory serviceFactory = serviceFactory;

    public object GetInstance(IServiceProvider serviceProvider)
    {
        return this.instances.GetOrAdd(serviceProvider.GetScope(), _ =>
        {
            object instance = this.serviceFactory(serviceProvider);
            serviceProvider.GetScope().Subscribe(this.ScopedDisposed);
            return instance;
        });
    }

    private void ScopedDisposed(IScope scope)
    {
        this.instances.Remove(scope, out _);
    }
}
