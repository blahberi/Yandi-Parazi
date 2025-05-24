using System.Collections.Concurrent;

namespace Cornflakes
{
    internal class ProviderOfServices : IProviderOfServices
    {
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> services;
        private bool isDisposed;

        public ProviderOfServices() 
        {
            this.services = new ConcurrentDictionary<Type, ServiceDescriptor>();
            this.Scope = new Scope(this);
        }

        public void Initialize()
        {
            foreach (ServiceDescriptor service in this.services.Values) service.LifetimeManager.Initialize(this);
        }

        public IScope Scope { get; }

        public void RegisterService(ServiceDescriptor descriptor)
        {
            if (!this.services.TryAdd(descriptor.ServiceType, descriptor))
            {
                throw new InvalidOperationException($"Can't register service of type {descriptor.ServiceType} since there is already a service registered with the same type.");
            }
        }

        public object GetService(Type serviceType)
        {
            if (this.services.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
            {
                return descriptor.LifetimeManager.GetInstance(this);
            }
            throw new KeyNotFoundException($"Service of type {serviceType} not found.");
        }

        public IScope CreateScope()
        {
            return this.CreateCopy().Scope;
        }

        public void Dispose()
        {
            if(this.isDisposed) return;
            this.isDisposed = true;
            this.Scope.Dispose();
        }

        private ProviderOfServices CreateCopy()
        {
            return new ProviderOfServices(this.services);
        }

        private ProviderOfServices(ConcurrentDictionary<Type, ServiceDescriptor> services)
        {
            this.services = services;
            this.Scope = new Scope(this);
        }
    }
}
