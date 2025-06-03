namespace Cornflakes;

internal class ScopeFactory : IScopeFactory
{
    public IScope CreateScope(IServiceProvider serviceProvider)
    {
        return new Scope(serviceProvider);
    }
}