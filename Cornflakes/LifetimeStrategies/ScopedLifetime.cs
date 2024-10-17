using System;
using System.Collections.Generic;

namespace Cornflakes.LifetimeStrategies
{
    internal class ScopedLifetime : ILfetimeStrategy
    {
        Dictionary<IScope, object> instances;

        private readonly ServiceFactory serviceFactory;

        public ScopedLifetime(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            IScope scope = serviceProvider.Scope;
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

        private void ScopeDisposed(IScope scope)
        {
            this.instances.Remove(scope);
            Console.WriteLine();
        }
    }
}
