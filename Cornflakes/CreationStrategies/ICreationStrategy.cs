namespace Cornflakes.CreationStrategies
{
    public delegate object ServiceFactory(IServiceProvider serviceProvider);
    public interface ICreationStrategy
    {
        object GetInstance(IServiceProvider serviceProvider);
    }
}
