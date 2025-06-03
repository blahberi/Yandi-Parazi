
namespace Cornflakes
{
    internal class Scope : IScope, IServiceProvider
    {
        private bool isDisposed;
        private List<ScopeDisposalHandler> disposalHandlers = [];
        private readonly IServiceProvider serviceProvider;


        public Scope(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider => this;
        
        public void Subscribe(ScopeDisposalHandler handler)
        {
            this.disposalHandlers.Add(handler);
        }

        public void Dispose()
        {
            if (this.isDisposed) return;
            this.isDisposed = true;
            this.InvokeDisposalEvent();
        }

        private void InvokeDisposalEvent()
        {
            this.disposalHandlers.ForEach(handler => handler(this));
        }

        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IScope))
            {
                return this;
            }
            return this.serviceProvider.GetService(serviceType);
        }
    }
}