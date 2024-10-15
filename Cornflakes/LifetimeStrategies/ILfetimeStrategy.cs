namespace Cornflakes.LifetimeStrategies
{
    public interface ILfetimeStrategy
    {
        object GetInstance(IServiceProvider serviceProvider);
    }
}
