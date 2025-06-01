namespace Cornflakes.LifetimeManagers
{
    public interface ILifetimeManager
    {
        void Initialize(IServiceProvider serviceProvider);
        object GetInstance(IProviderOfServices serviceProvider);
    }
}
