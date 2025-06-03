namespace Cornflakes.LifetimeManagers
{
    internal class TransientLifetime : ILifetimeManager
    {
        private readonly IServiceCreationPipeline creationPipeline;
        public TransientLifetime(IServiceCreationPipeline creationPipeline) 
        {
            this.creationPipeline = creationPipeline;
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            this.creationPipeline.Invoke(serviceProvider, out object instance);
            return instance;
        }
    }
}
