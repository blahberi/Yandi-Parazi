namespace Cornflakes;

public interface IServiceFactory
{
    IServiceContainer Create(IServiceProvider serviceProvider);
}