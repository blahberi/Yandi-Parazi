namespace Yandi.Core.Services;

internal class ServiceProviderFactory(IServiceCollection services) : IServiceProviderFactroy
{
    private readonly IServiceCollection services = new ServiceCollection(services);

    public IServiceProvider Create()
    {
        return new Container(this.services);
    }
}
