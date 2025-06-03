using System.Collections.Concurrent;
using Cornflakes.Extensions;

namespace Cornflakes.LifetimeManagers
{
    internal class ScopedLifetime : ILifetimeManager
    {
        private readonly ConcurrentDictionary<IScope, IServiceContainer> containers;
        private readonly IServiceFactory serviceFactory;

        public ScopedLifetime(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            this.containers = new ConcurrentDictionary<IScope, IServiceContainer>();
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            return this.containers.GetOrAdd(serviceProvider.GetScope(), _ =>
            {
                IServiceContainer container = this.serviceFactory.Create(serviceProvider);
                serviceProvider.GetScope().Subscribe(this.scopedDisposed);
                return container;
            }).GetService(serviceProvider);
        }

        private void scopedDisposed(IScope scope)
        {
            this.containers.Remove(scope, out _);
        }
    }
}
