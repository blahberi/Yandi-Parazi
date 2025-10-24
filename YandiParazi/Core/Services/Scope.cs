namespace Yandi.Core.Services;

internal class Scope(IServiceProvider serviceProvider) : IScope
{
    private bool isDisposed;
    private readonly List<ScopeDisposalHandler> disposalHandlers = [];

    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public void Subscribe(ScopeDisposalHandler handler)
    {
        this.disposalHandlers.Add(handler);
    }

    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.isDisposed = true;
        this.InvokeDisposalEvent();
    }

    private void InvokeDisposalEvent()
    {
        this.disposalHandlers.ForEach(handler => handler(this));
    }
}
