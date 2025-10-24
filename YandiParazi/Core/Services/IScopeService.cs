namespace Yandi.Core.Services;

public interface IScopeService
{
    IScope CreateScope();
    IScope GetScope(IServiceProvider scopedProvider);
}
