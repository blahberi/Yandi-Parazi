namespace Cornflakes;


public delegate object ServiceFactory(IProviderOfServices serviceProvider);
public delegate void ServiceCreationPipeline(IProviderOfServices serviceProvider, out object instance);

public static class ServiceFactoryExtensions
{
    public static ServiceCreationPipeline ToPipeline(this ServiceFactory factory)
    {
        return (IProviderOfServices serviceProvider, out object instance) =>
        {
            instance = factory(serviceProvider);
        };
    }
    
    public static ServiceFactory ToFactory(this ServiceCreationPipeline creationPipeline)
    {
        return (IProviderOfServices serviceProvider) =>
        {
            creationPipeline(serviceProvider, out object instance);
            return instance;
        };
    }

    public static ServiceCreationPipeline Attach(this ServiceCreationPipeline creationPipeline, ServiceCreationPipeline other)
    {
        return (IProviderOfServices provider, out object instance) =>
        {
            creationPipeline(provider, out object obj);
            instance = obj;
            other(provider, out instance);
        };
    }
}