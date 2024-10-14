using System;
using System.Collections.Generic;

namespace Cornflakes.CreationStrategies
{
    internal class ScopedCreation : ICreationStrategy
    {
        Dictionary<IServiceProvider, object> instances;

        private readonly ServiceFactory serviceFactory;

        public ScopedCreation(ServiceFactory serviceFactory)
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
