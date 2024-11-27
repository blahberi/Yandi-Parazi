namespace Cornflakes
{
    internal class Scope : IScope
    {
        private bool isDisposed = false;
        private List<ScopeDisposalHandler> disposalHandlers = new List<ScopeDisposalHandler>();


        public Scope(IProviderOfServices serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
        }

        public IProviderOfServices ServiceProvider { get; }

        public void Subscribe(ScopeDisposalHandler handler)
        {
            disposalHandlers.Add(handler);
        }

        public void Dispose()
        {
            if (this.isDisposed) return;
            this.isDisposed = true;
            this.ServiceProvider.Dispose();
            this.InvokeDisposalEvent();
        }

        private void InvokeDisposalEvent()
        {
            disposalHandlers.ForEach(handler => handler(this));
        }
    }
}