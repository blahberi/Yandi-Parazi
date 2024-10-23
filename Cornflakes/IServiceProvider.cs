namespace Cornflakes
{
    public interface IServiceProvider : IDisposable
    {
        IScope Scope { get; }
        object GetService(Type serviceType);
        IScope CreateScope();
    }
}
