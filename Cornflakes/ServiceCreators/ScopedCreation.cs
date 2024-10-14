using System;
using System.Collections.Generic;

namespace Cornflakes
{
    internal class ScopedCreation : BaseCreationStrategy
    {
        Dictionary<IServiceProvider, object> instances;
        public override object GetInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            if (instances == null)
            {
                instances = new Dictionary<IServiceProvider, object>();
            }

            if (instances.ContainsKey(serviceProvider))
            {
                return instances[serviceProvider];
            }

            object instance = CreateInstance(implementationType, serviceProvider);
            instances.Add(serviceProvider, instance);
            return instance;
        }
    }
}
