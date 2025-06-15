namespace Cornflakes.ServiceCreation;

public interface IServiceContainer
{
    object GetService(IServiceProvider serviceProvider);
}