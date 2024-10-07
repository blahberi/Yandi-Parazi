using System;

namespace ZeTaim
{
    public class ServiceDescriptor
    {
        public Type ServiceType;
        public Type ServiceImplementation;
        
        public ServiceDescriptor(Type serviceType, Type serviceImplementation)
        {
            this.ServiceType = serviceType;
            this.ServiceImplementation = serviceImplementation;
        }
    }
}
