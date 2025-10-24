namespace Yandi
{
    public class ServiceDescriptor(Type serviceType, ILifetimeManager lifetimeManager)
    {
        public Type ServiceType { get; } = serviceType;
        public ILifetimeManager LifetimeManager { get; } = lifetimeManager;
    }
}
