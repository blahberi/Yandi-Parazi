using Cornflakes.CreationStrategies;

namespace Cornflakes
{
    public interface IServiceProviderBuilder
    {
        IServiceProviderBuilder RegisterService<TService, TImplementation>(ICreationStrategy creationStrategy) where TImplementation : TService;
        IServiceProviderBuilder RegisterServices(IServiceCollection services);
        IServiceProvider Build();
    }
}
