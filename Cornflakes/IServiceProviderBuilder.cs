using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public interface IServiceProviderBuilder
    {
        IServiceProviderBuilder RegisterService<TService, TImplementation>(ILfetimeStrategy creationStrategy) where TImplementation : TService;
        IServiceProviderBuilder RegisterServices(IServiceCollection services);
        IServiceProvider Build();
    }
}
