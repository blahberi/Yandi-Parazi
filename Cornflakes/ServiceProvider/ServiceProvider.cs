using System.Collections.Concurrent;

namespace Cornflakes;

internal class ServiceProvider: IServiceProvider
{
    private readonly ConcurrentDictionary<Type, ServiceDescriptor> services;

    public ServiceProvider(IServiceCollection services) 
    {
        this.services = new ConcurrentDictionary<Type, ServiceDescriptor>();
        foreach (ServiceDescriptor service in services)
        {
            this.services[service.ServiceType] = service;
        }
    }

    public object? GetService(Type serviceType)
    {
        if (this.services.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
        {
            return descriptor.LifetimeManager.GetInstance(this);
        }

        return null;
    }
}
