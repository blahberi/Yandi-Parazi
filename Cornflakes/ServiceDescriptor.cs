using Cornflakes.LifetimeManagers;

namespace Cornflakes
{
    public class ServiceDescriptor
    {
        public ServiceDescriptor(Type serviceType, ILifetimeManager lifetimeManager)
        {
            this.ServiceType = serviceType;
            this.LifetimeManager = lifetimeManager;
        }

        public Type ServiceType { get; }
        public ILifetimeManager LifetimeManager { get; }
    }
}
