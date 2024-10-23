using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        IServiceCollection AddService<TService>(ILifetimeStrategy creationStrategy);
    }
}
