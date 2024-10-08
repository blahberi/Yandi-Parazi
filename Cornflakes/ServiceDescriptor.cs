using System;

namespace ZeTaim
{
    public class ServiceDescriptor
    {
        public ServiceDescriptor(Type serviceType, Type serviceImplementation)
        {
            this.ServiceType = serviceType;
            this.ServiceImplementation = serviceImplementation;
        }

        public Type ServiceType { get; set; }
        public Type ServiceImplementation { get; }
    }
}
