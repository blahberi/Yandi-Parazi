using System.Collections.Generic;

namespace Cornflakes.LifetimeStrategies
{
    internal class ScopedLifetime : ILfetimeStrategy
    {
        Dictionary<IServiceProvider, object> instances;

        private readonly ServiceFactory serviceFactory;

        public ScopedLifetime(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            if (instances == null)
            {
                instances = new Dictionary<IServiceProvider, object>();
            }

            if (instances.ContainsKey(serviceProvider))
            {
                return instances[serviceProvider];
            }

            object instance = serviceFactory(serviceProvider);
            instances.Add(serviceProvider, instance);
            return instance;
        }
    }
}
