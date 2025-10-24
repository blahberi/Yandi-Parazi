namespace Yandi;

public interface ILifetimeManager
{
    object GetInstance(IServiceProvider serviceProvider);
}
