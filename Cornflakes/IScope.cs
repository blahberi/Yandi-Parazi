namespace Cornflakes
{
    public delegate void ScopeDisposalHandler(IScope scope);
    public interface IScope : IDisposable
    {
        IProviderOfServices ServiceProvider { get; }
        void Subscribe(ScopeDisposalHandler handler);
    }
}