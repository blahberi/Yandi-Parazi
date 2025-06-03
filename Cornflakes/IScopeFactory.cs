namespace Cornflakes
{
    internal interface IScopeFactory
    {
        IScope CreateScope(IServiceProvider serviceProvider);
    }
}
