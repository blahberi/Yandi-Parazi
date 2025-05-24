namespace Cornflakes.LifetimeManagers
{
    internal class SingletonLifetime : ILifetimeManager
    {
        private object? instance;
        private readonly ServiceLoader _serviceLoader;
        private readonly object lockObject = new object();

        public SingletonLifetime(ServiceLoader serviceLoader)
        {
            this._serviceLoader = serviceLoader;
        }

        private bool Initialized => this.instance != null;

        public void Initialize(IProviderOfServices serviceProvider)
        {
            if (this.Initialized) return;
            lock (this.lockObject)
            {
                if (this.Initialized) return;
                this._serviceLoader(serviceProvider, out this.instance);
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
