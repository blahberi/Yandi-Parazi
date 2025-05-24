namespace Cornflakes.LifetimeManagers
{
    public interface ILifetimeManager
    {
        void Initialize(IProviderOfServices serviceProvider);
        object GetInstance(IProviderOfServices serviceProvider);
    }
}
