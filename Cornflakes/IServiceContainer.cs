using Cornflakes.LifetimeManagers;

namespace Cornflakes;

public interface IServiceContainer
{
    object GetService(IServiceProvider serviceProvider);
}