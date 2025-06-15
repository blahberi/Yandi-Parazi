using System.Collections.Concurrent;

namespace Cornflakes.Scopes;

internal class ScopeService : IScopeService
{
    private readonly IServiceProviderFactroy serviceProviderFactory;
    private readonly ConcurrentDictionary<IServiceProvider, IScope> scopes = new();
    
    public ScopeService(IServiceProviderFactroy serviceProviderFactory)
    {
        this.serviceProviderFactory = serviceProviderFactory;
    }
    
    public IScope CreateScope()
    {
        return this.CreateScope(this.serviceProviderFactory.Create());
    }

    public IScope GetScope(IServiceProvider scopedProvider)
    {
        return this.scopes.TryGetValue(scopedProvider, out IScope? scope) ? scope : this.CreateScope(scopedProvider);
    }

    private IScope CreateScope(IServiceProvider scopedProvider)
    {
        IScope scope = new Scope(scopedProvider);
        this.scopes.TryAdd(scopedProvider, scope);
        scope.Subscribe(_ =>
        {
            this.scopes.Remove(scopedProvider, out IScope _);
        });
        return scope;
    }
}