namespace Cornflakes;

internal interface IScopeService
{
    IScope CreateScope();
    IScope GetScope(IServiceProvider scopedProvider);
}