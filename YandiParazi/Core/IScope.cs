namespace Yandi.Core;

public delegate void ScopeDisposalHandler(IScope scope);
public interface IScope : IDisposable
{
    IServiceProvider ServiceProvider { get; }
    void Subscribe(ScopeDisposalHandler handler);
}
