namespace Cornflakes.LifetimeManagers
{
    internal class TransientLifetime : ILifetimeManager
    {
        private readonly ServiceCreationPipeline _serviceCreationPipeline;
        public TransientLifetime(ServiceCreationPipeline serviceCreationPipeline) 
        {
            this._serviceCreationPipeline = serviceCreationPipeline;
        }
        
        public void Initialize(IProviderOfServices serviceProvider) {}

        public object GetInstance(IProviderOfServices serviceProvider)
        {
            this._serviceCreationPipeline(serviceProvider, out object instance);
            return instance;
        }
    }
}
