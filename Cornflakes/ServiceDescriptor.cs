using Cornflakes.LifetimeStrategies;
using System;

namespace Cornflakes
{
    public class ServiceDescriptor
    {
        public ServiceDescriptor(Type serviceType, ILfetimeStrategy lifetimeStrategy)
        {
            this.ServiceType = serviceType;
            this.LifetimeStrategy = lifetimeStrategy;
        }

        public Type ServiceType { get; set; }
        public ILfetimeStrategy LifetimeStrategy { get; set; }
    }
}
