using Cornflakes.CreationStrategies;
using System;

namespace Cornflakes
{
    public class ServiceDescriptor
    {
        public ServiceDescriptor(Type serviceType, Type serviceImplementation, ICreationStrategy creationStrategy)
        {
            this.ServiceType = serviceType;
            this.ServiceImplementation = serviceImplementation;
            this.CreationStrategy = creationStrategy;
        }

        public Type ServiceType { get; set; }
        public Type ServiceImplementation { get; }
        public ICreationStrategy CreationStrategy { get; set; }
    }
}
