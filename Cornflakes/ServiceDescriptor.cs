using Cornflakes.LifetimeStrategies;

namespace Cornflakes
{
    public class ServiceDescriptor
    {
        public ServiceDescriptor(Type serviceType, ILifetimeStrategy lifetimeStrategy)
        {
            this.ServiceType = serviceType;
            this.LifetimeStrategy = lifetimeStrategy;
        }

        public Type ServiceType { get; set; }
        public ILifetimeStrategy LifetimeStrategy { get; set; }
    }
}
