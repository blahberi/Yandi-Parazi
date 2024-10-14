using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cornflakes
{
    internal class ServiceProvider : IServiceProvider
    {
        private readonly IDictionary<Type, ServiceDescriptor> services;

        public ServiceProvider() 
        {
            services = new Dictionary<Type, ServiceDescriptor>();
            this.Scope = new Scope(this);
        }

        public ServiceProvider(IDictionary<Type, ServiceDescriptor> services)
        {
            this.services = services;
            this.Scope = new Scope(this);
        }

        public IScope Scope { get; }

        public void RegisterService(ServiceDescriptor descriptor)
        {
            this.services[descriptor.ServiceType] = descriptor;
        }

        public object GetService(Type serviceType)
        {
            Type implementationType = this.services[serviceType].ServiceImplementation;
            return this.services[serviceType].CreationStrategy.GetInstance(implementationType, this);
        }

        public IScope CreateChildScope()
        {
            return new Scope(CreateCopy());
        }


        private IServiceProvider CreateCopy()
        {
            return new ServiceProvider(services);
        }
    }
}
