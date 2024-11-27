using System.Collections.Concurrent;

namespace Cornflakes
{
    internal class ProviderOfServices : IProviderOfServices
    {
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> services;
        private bool isDisposed;

        public ProviderOfServices() 
        {
            services = new ConcurrentDictionary<Type, ServiceDescriptor>();
            this.Scope = new Scope(this);
        }

        public IScope Scope { get; }

        public void RegisterService(ServiceDescriptor descriptor)
        {
            if (this.services.ContainsKey(descriptor.ServiceType))
            {
                throw new InvalidOperationException($"Can't register service of type {descriptor.ServiceType} since there is already a service registered with the same type.");
            }
            this.services[descriptor.ServiceType] = descriptor;
        }

        public object GetService(Type serviceType)
        {
            if (this.services.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
            {
                return descriptor.LifetimeStrategy.GetInstance(this);
            }
            throw new KeyNotFoundException($"Service of type {serviceType} not found.");
        }

        public IScope CreateScope()
        {
            return CreateCopy().Scope;
        }

        public void Dispose()
        {
            if(this.isDisposed) return;
            this.isDisposed = true;
            this.Scope.Dispose();
        }

        private IProviderOfServices CreateCopy()
        {
            return new ProviderOfServices(services);
        }

        private ProviderOfServices(ConcurrentDictionary<Type, ServiceDescriptor> services)
        {
            this.services = services;
            this.Scope = new Scope(this);
        }
    }
}
