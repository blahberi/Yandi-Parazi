namespace Cornflakes
{
    public interface IProviderOfServices : IDisposable
    {
        IScope Scope { get; }
        object GetService(Type serviceType);
        IScope CreateScope();
    }
}
