using System.Collections.Concurrent;

namespace Yandi.Core.Services;

internal class ServiceProvider : IServiceProvider
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
        return this.services.TryGetValue(serviceType, out ServiceDescriptor? descriptor)
            ? descriptor.LifetimeManager.GetInstance(this)
            : null;
    }
}
