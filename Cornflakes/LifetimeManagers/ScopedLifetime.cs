using System.Collections.Concurrent;
using Cornflakes.Extensions;

namespace Cornflakes.LifetimeManagers
{
    internal class ScopedLifetime : ILifetimeManager
    {
        private readonly ConcurrentDictionary<IScope, object> instances;
        private readonly IServiceCreationPipeline creationPipeline;

        public ScopedLifetime(IServiceCreationPipeline creationPipeline)
        {
            this.creationPipeline = creationPipeline;
            this.instances = new ConcurrentDictionary<IScope, object>();
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            return this.instances.GetOrAdd(serviceProvider.GetScope(), _ =>
            {
                this.creationPipeline.Invoke(serviceProvider, out object instance);
                serviceProvider.GetScope().Subscribe(this.scopedDisposed);
                return instance;
            });
        }

        private void scopedDisposed(IScope scope)
        {
            this.instances.Remove(scope, out _);
        }
    }
}
