using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public interface IServiceProviderBuilder
    {
        IServiceProviderBuilder RegisterService<TService>(ILifetimeStrategy creationStrategy); 
        IServiceProviderBuilder RegisterServices(IServiceCollection services);
        IProviderOfServices Build();
    }
}
