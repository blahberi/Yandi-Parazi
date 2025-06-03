namespace Cornflakes;

public class GlobalScope : IScope
{
    public GlobalScope(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
        this.Scope = new Scope(serviceProvider);
    }
    public IServiceProvider ServiceProvider { get; } 
    private IScope Scope { get; }
    public void Subscribe(ScopeDisposalHandler handler)
    {
        this.Scope.Subscribe(handler);
    }
    
    public void Dispose()
    {
        this.Scope.Dispose();
    }
}