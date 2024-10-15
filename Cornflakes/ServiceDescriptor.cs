using Cornflakes.LifetimeStrategies;
using System;

namespace Cornflakes
{
    public class ServiceDescriptor
    {
        public ServiceDescriptor(Type serviceType, Type serviceImplementation, ILfetimeStrategy lifetimeStrategy)
        {
            this.ServiceType = serviceType;
            this.ServiceImplementation = serviceImplementation;
            this.LifetimeStrategy = lifetimeStrategy;
        }

        public Type ServiceType { get; set; }
        public Type ServiceImplementation { get; }
        public ILfetimeStrategy LifetimeStrategy { get; set; }
    }
}
