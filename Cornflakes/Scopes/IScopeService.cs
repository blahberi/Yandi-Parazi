namespace Cornflakes.Scopes;

internal interface IScopeService
{
    IScope CreateScope();
    IScope GetScope(IServiceProvider scopedProvider);
}