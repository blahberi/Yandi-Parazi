namespace Cornflakes.CreationStrategies
{
    public interface ICreationStrategy
    {
        object GetInstance(IServiceProvider serviceProvider);
    }
}
