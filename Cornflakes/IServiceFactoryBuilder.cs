namespace Cornflakes;

public delegate object ServiceCreator(IServiceProvider serviceProvider);
public delegate object ServiceCreationWrapper(IServiceProvider serviceProvider, object instance);
public delegate void ServiceInitializer(IServiceProvider serviceProvider, object instance);
public interface IServiceFactoryBuilder
{
    IServiceFactoryBuilder Add(ServiceCreationWrapper serviceProviderWrapper);
    IServiceFactoryBuilder Add(ServiceInitializer serviceInitializer);
    IServiceFactory Build();
}