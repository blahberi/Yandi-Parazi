namespace Cornflakes;

public interface IServiceCreationPipeline
{
    void Invoke(IServiceProvider serviceProvider, out object instance);
}