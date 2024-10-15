namespace Cornflakes.LifetimeStrategies
{
    internal class SingletonLifetime : ILfetimeStrategy
    {
        private object instance;
        private readonly ServiceFactory serviceFactory;

        public SingletonLifetime(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            if (this.instance == null)
            {
                this.instance = this.serviceFactory(serviceProvider);
            }
            return this.instance;
        }
    }
}
