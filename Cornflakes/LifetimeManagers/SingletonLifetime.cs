namespace Cornflakes.LifetimeManagers
{
    internal class SingletonLifetime : ILifetimeManager
    {
        private object? instance;
        private readonly ServiceCreationPipeline _serviceCreationPipeline;
        private readonly object lockObject = new object();

        public SingletonLifetime(ServiceCreationPipeline serviceCreationPipeline)
        {
            this._serviceCreationPipeline = serviceCreationPipeline;
        }

        private bool Initialized => this.instance != null;

        public void Initialize(IProviderOfServices serviceProvider)
        {
            if (this.Initialized) return;
            lock (this.lockObject)
            {
                if (this.Initialized) return;
                this._serviceCreationPipeline(serviceProvider, out this.instance);
            }
        }

        public object GetInstance(IProviderOfServices serviceProvider)
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
