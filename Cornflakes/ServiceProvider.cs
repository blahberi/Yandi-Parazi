using System.Collections.Concurrent;

namespace Cornflakes
{
    internal class ServiceProvider: IServiceProvider
    {
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> services;

        public ServiceProvider(IServiceCollection services) 
        {
            this.services = new ConcurrentDictionary<Type, ServiceDescriptor>();
            
            foreach (ServiceDescriptor service in services)
            {
                if (!this.services.TryAdd(service.ServiceType, service))
                {
                    throw new InvalidOperationException($"Can't register service of type {service.ServiceType} since there is already a service registered with the same type.");
                }
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
    }
}
