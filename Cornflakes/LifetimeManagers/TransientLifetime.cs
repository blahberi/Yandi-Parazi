namespace Cornflakes.LifetimeManagers
{
    internal class TransientLifetime : ILifetimeManager
    {
        private readonly ServiceLoader _serviceLoader;
        public TransientLifetime(ServiceLoader serviceLoader) 
        {
            this._serviceLoader = serviceLoader;
        }
        
        public void Initialize(IProviderOfServices serviceProvider) {}

        public object GetInstance(IProviderOfServices serviceProvider)
        {
            this._serviceLoader(serviceProvider, out object instance);
            return instance;
        }
    }
}
