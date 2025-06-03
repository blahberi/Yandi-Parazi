namespace Cornflakes.LifetimeManagers
{
    internal class SingletonLifetime : ILifetimeManager
    {
        private object? instance;
        private readonly IServiceCreationPipeline creationPipeline;
        private readonly object lockObject = new object();

        public SingletonLifetime(IServiceCreationPipeline creationPipeline)
        {
            this.creationPipeline = creationPipeline;
        }

        private bool Initialized => this.instance != null;

        public void Initialize(IServiceProvider serviceProvider)
        {
            if (this.Initialized) return;
            lock (this.lockObject)
            {
                if (this.Initialized) return;
                this.creationPipeline.Invoke(serviceProvider, out this.instance);
            }
        }

        public object GetInstance(IServiceProvider serviceProvider)
        {
            if (this.Initialized) return this.instance!;
            lock (this.lockObject)
            {
                if (!this.Initialized) this.Initialize(serviceProvider);
                return this.instance!;
            }
        }
    }
}
