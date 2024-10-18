using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public interface IServiceProviderBuilder
    {
        IServiceProviderBuilder RegisterService<TService>(ILfetimeStrategy creationStrategy); 
        IServiceProviderBuilder RegisterServices(IServiceCollection services);
        IServiceProvider Build();
    }
}
