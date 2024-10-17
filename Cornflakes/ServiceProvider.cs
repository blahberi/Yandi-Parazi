using System;
using System.Collections.Generic;

namespace Cornflakes
{
    internal class ServiceProvider : IServiceProvider
    {
        private readonly IDictionary<Type, ServiceDescriptor> services;
        private bool isDisposed;

        public ServiceProvider() 
        {
            services = new Dictionary<Type, ServiceDescriptor>();
            this.Scope = new Scope(this);
        }


        public IScope Scope { get; }

        public ServiceProvider(IDictionary<Type, ServiceDescriptor> services)
        {
            this.services = services;
            this.Scope = new Scope(this);
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
            return CreateCopy().Scope;
        }

        public ServiceDescriptor GetDescriptor(Type serviceType)
        {
            return services[serviceType];
        }
        public void RemoveService(ServiceDescriptor desciptor)
        {
            services.Remove(desciptor.ServiceType);
        }

        public void Dispose()
        {
            if(this.isDisposed) return;
            this.isDisposed = true;
            this.Scope.Dispose();
        }

        private IServiceProvider CreateCopy()
        {
            return new ServiceProvider(services);
        }
    }
}
