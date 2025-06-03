namespace Cornflakes.LifetimeManagers
{
    public interface ILifetimeManager
    {
        object GetInstance(IServiceProvider serviceProvider);
    }
}
