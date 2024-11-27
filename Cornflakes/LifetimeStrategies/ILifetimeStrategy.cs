namespace Cornflakes.LifetimeStrategies
{
    public interface ILifetimeStrategy
    {
        object GetInstance(IProviderOfServices serviceProvider);
    }
}
