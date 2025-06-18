namespace Yandi;

internal class ServiceProviderFactory : IServiceProviderFactroy
{
    private readonly IServiceCollection services;
    public ServiceProviderFactory(IServiceCollection services)
    {
        this.services = services;
    }

    public IServiceProvider Create()
    {
        return new ServiceProvider(this.services);
    }
}