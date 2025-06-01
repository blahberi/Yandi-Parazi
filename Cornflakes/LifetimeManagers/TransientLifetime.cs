namespace Cornflakes.LifetimeManagers
{
    internal class TransientLifetime : ILifetimeManager
    {
        private readonly IServiceCreationPipeline creationPipeline;
        public TransientLifetime(IServiceCreationPipeline creationPipeline) 
        {
            this.creationPipeline = creationPipeline;
        }
        
        public void Initialize(IProviderOfServices serviceProvider) {}

        public object GetInstance(IProviderOfServices serviceProvider)
        {
            this.creationPipeline.Invoke(serviceProvider, out object instance);
            return instance;
        }
    }
}
