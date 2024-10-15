using System;
using System.Collections.Generic;

namespace Cornflakes
{
    internal class ServiceProvider : IServiceProvider
    {
        private readonly IDictionary<Type, ServiceDescriptor> services;

        public ServiceProvider() 
        {
            services = new Dictionary<Type, ServiceDescriptor>();
        }

        public ServiceProvider(IDictionary<Type, ServiceDescriptor> services)
        {
            this.services = services;
        }
        public void RegisterService(ServiceDescriptor descriptor)
        {
            this.services[descriptor.ServiceType] = descriptor;
        }

        public object GetService(Type serviceType)
        {
            Type implementationType = this.services[serviceType].ServiceImplementation;
            return this.services[serviceType].LifetimeStrategy.GetInstance(this);
        }

        public IScope CreateScope()
        {
            return new Scope(CreateCopy());
        }

        private IServiceProvider CreateCopy()
        {
            return new ServiceProvider(services);
        }

        public void Dispose()
        {
        }
    }
}
