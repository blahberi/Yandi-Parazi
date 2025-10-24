using System.Collections.Concurrent;

namespace Yandi.Core.Services;

internal class ScopeService(IServiceProviderFactroy serviceProviderFactory) : IScopeService
{
    private readonly IServiceProviderFactroy serviceProviderFactory = serviceProviderFactory;
    private readonly ConcurrentDictionary<IServiceProvider, IScope> scopes = new();

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
        using IScope scope = new Scope(scopedProvider);
        this.scopes.TryAdd(scopedProvider, scope);
        scope.Subscribe(_ =>
        {
            this.scopes.Remove(scopedProvider, out _);
        });
        return scope;
    }
}
