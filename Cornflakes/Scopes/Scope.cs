namespace Cornflakes.Scopes;

internal class Scope : IScope
{
    private bool isDisposed;
    private List<ScopeDisposalHandler> disposalHandlers = [];

    public Scope(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public IServiceProvider ServiceProvider { get; }
        
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
}