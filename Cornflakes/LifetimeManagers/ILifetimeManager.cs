namespace Cornflakes.LifetimeManagers
{
    public interface ILifetimeManager
    {
        IServiceContainer GetInstance(IServiceProvider serviceProvider);
    }
}
