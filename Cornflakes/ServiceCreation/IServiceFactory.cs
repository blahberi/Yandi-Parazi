namespace Cornflakes.ServiceCreation;

public interface IServiceFactory
{
    IServiceContainer Create(IServiceProvider serviceProvider);
}