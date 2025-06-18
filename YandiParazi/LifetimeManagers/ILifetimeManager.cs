namespace Yandi.LifetimeManagers;

public interface ILifetimeManager
{
    object GetInstance(IServiceProvider serviceProvider);
}
