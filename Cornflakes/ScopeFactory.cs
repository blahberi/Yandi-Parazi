namespace Cornflakes;

internal class ScopeFactory : IScopeFactory
{
    public IScope CreateScope()
    {
        return new Scope();
    }
}