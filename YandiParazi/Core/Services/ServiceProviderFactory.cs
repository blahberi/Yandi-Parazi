namespace Yandi.Core.Services;

internal class ServiceProviderFactory(IServiceCollection services) : IServiceProviderFactroy
{
    private readonly IServiceCollection services = services;

    public IServiceProvider Create()
    {
        return new ServiceProvider(this.services);
    }
}
