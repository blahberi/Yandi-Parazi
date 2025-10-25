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

    public IScope GetScope(IServiceProvider serviceProvider)
    {
        return this.scopes.TryGetValue(serviceProvider, out IScope? scope)
            ? scope : this.CreateScope(serviceProvider);
    }

    private IScope CreateScope(IServiceProvider serviceProvider)
    {
        using IScope scope = new Scope(serviceProvider);
        this.scopes.TryAdd(serviceProvider, scope);
        scope.Subscribe(_ =>
        {
            this.scopes.Remove(serviceProvider, out _);
        });
        return scope;
    }
}
