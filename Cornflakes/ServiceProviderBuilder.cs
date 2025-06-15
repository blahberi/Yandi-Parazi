using Cornflakes.Extensions;
using Cornflakes.LifetimeManagers;

namespace Cornflakes;

public class ServiceProviderBuilder : IServiceProviderBuilder
{
    private readonly ServiceCollection services = [];

    public ServiceProviderBuilder()
    {
        this.services
            .AddTransient<IServiceProvider>(sp => sp)
            .AddSingleton<IServiceProviderFactroy>(sp => new ServiceProviderFactory(this.services))
            .AddSingleton<IScopeService, ScopeService>();
    }

    public IServiceProviderBuilder RegisterService<TService>(ILifetimeManager lifetimeManager)
    {
        this.services.AddService<TService>(lifetimeManager);
        return this;
    }

    public IServiceProviderBuilder RegisterServices(IServiceCollection services)
    {
        foreach (ServiceDescriptor service in services)
        {
            this.services.Add(service);
        }
        return this;
    }

    public IServiceProvider Build()
    {
        this.services.IsReadOnly = true;
        return new ServiceProvider(this.services);
    }
}
