namespace Yandi.Scopes;

internal interface IScopeService
{
    IScope CreateScope();
    IScope GetScope(IServiceProvider scopedProvider);
}