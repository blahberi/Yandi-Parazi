namespace Cornflakes;


public delegate object ServiceFactory(IProviderOfServices serviceProvider);
public delegate void ServiceLoader(IProviderOfServices serviceProvider, out object instance);

public static class ServiceFactoryExtensions
{
    public static ServiceLoader ToLoader(this ServiceFactory factory)
    {
        return (IProviderOfServices serviceProvider, out object instance) =>
        {
            instance = factory(serviceProvider);
        };
    }
}