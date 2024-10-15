namespace Cornflakes.LifetimeStrategies
{
    internal class TransientLifetime : ILfetimeStrategy 
    {
        private readonly ServiceFactory serviceFactory;
        public TransientLifetime(ServiceFactory serviceFactory) 
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            return this.serviceFactory(serviceProvider);
        }
    }
}
