using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public interface IServiceProviderBuilder
    {
        IServiceProviderBuilder RegisterService<TService>(ILifetimeManager creationStrategy); 
        IServiceProviderBuilder RegisterServices(IServiceCollection services);
        IProviderOfServices Build();
    }
}
