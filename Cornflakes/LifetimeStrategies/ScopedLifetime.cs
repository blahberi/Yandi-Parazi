using System;
using System.Collections.Generic;

namespace Cornflakes.LifetimeStrategies
{
    internal class ScopedLifetime : ILifetimeStrategy
    {
        Dictionary<IScope, object> instances;
        private readonly ServiceFactory serviceFactory;
        private readonly object lockObject = new object();

        public ScopedLifetime(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            IScope scope = serviceProvider.Scope;
            lock(this.lockObject)
            {
                if (instances == null)
                {
                    instances = new Dictionary<IScope, object>();
                }

                if (instances.ContainsKey(scope))
                {
                    return instances[scope];
                }

                object instance = serviceFactory(serviceProvider);
                instances.Add(scope, instance);
                scope.Subscribe(ScopeDisposed);
                return instance;
            }
        }

        private void ScopeDisposed(IScope scope)
        {
            lock(this.lockObject)
            {
                if (!instances.ContainsKey(scope)) return;
                this.instances.Remove(scope);
            }
        }
    }
}
