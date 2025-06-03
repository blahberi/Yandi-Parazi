namespace Cornflakes.LifetimeManagers
{
    internal class TransientLifetime : ILifetimeManager
    {
        private readonly IServiceFactory serviceFactory;
        public TransientLifetime(IServiceFactory serviceFactory) 
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            return this.serviceFactory.Create(serviceProvider).GetService(serviceProvider);
        }
    }
}
