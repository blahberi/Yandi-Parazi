using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        IServiceCollection AddService<TService>(ILifetimeManager lifetimeManager);
    }
}
