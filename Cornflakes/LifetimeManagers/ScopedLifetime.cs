namespace Cornflakes.LifetimeManagers
{
    internal class ScopedLifetime : ILifetimeManager
    {
        Dictionary<IScope, object> instances;
        private readonly ServiceLoader _serviceLoader;
        private readonly object lockObject = new object();

        public ScopedLifetime(ServiceLoader serviceLoader)
        {
            this._serviceLoader = serviceLoader;
            this.instances = new Dictionary<IScope, object>();
        }
        
        private bool Initialized
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.instances != null;
                }
            }
        }

        public void Initialize(IProviderOfServices serviceProvider)
        {
            if (this.Initialized) return;
            lock (this.lockObject)
            {
                this.instances = new Dictionary<IScope, object>();
                this.registerInstance(serviceProvider);
            }
        }

        public object GetInstance(IProviderOfServices serviceProvider)
        {
            if (!this.Initialized) this.Initialize(serviceProvider);
            
            lock(this.lockObject)
            {
                return this.registerInstance(serviceProvider);
            }
        }

        private void scopedDisposed(IScope scope)
        {
            lock(this.lockObject)
            {
                if (!this.instances.ContainsKey(scope)) return;
                this.instances.Remove(scope);
            }
        }

        private object registerInstance(IProviderOfServices serviceProvider)
        {
            IScope scope = serviceProvider.Scope;
            this._serviceLoader(serviceProvider, out object instance);
            this.instances.Add(scope, instance);
            scope.Subscribe(this.scopedDisposed);
            return instance;
        }
    }
}
