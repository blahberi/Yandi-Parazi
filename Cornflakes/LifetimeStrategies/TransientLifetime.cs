namespace Cornflakes.LifetimeStrategies
{
    internal class TransientLifetime : ILifetimeStrategy 
    {
        private readonly ServiceFactory serviceFactory;
        public TransientLifetime(ServiceFactory serviceFactory) 
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IProviderOfServices serviceProvider)
        {
            return this.serviceFactory(serviceProvider);
        }
    }
}
