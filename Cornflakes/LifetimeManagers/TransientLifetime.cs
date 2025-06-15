using Cornflakes.ServiceCreation;

namespace Cornflakes.LifetimeManagers
{
    internal class TransientLifetime : ILifetimeManager
    {
        private readonly IServiceFactory serviceFactory;
        public TransientLifetime(IServiceFactory serviceFactory) 
        {
            this.serviceFactory = serviceFactory;
        }

        public IServiceContainer GetInstance(IServiceProvider serviceProvider)
        {
            return this.serviceFactory.Create(serviceProvider);
        }
    }
}
