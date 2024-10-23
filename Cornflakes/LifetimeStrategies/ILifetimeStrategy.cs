namespace Cornflakes.LifetimeStrategies
{
    public interface ILifetimeStrategy
    {
        object GetInstance(IServiceProvider serviceProvider);
    }
}
