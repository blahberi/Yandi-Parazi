namespace Cornflakes.ServiceCreation;

public delegate object ServiceCreator(IServiceProvider serviceProvider);
public delegate void ServiceInitializer(IServiceProvider serviceProvider, object instance);
public delegate IServiceContainer ServiceFactory(IServiceProvider serviceProvider);
public interface IServiceFactoryBuilder
{
    IServiceFactoryBuilder Add(ServiceInitializer serviceInitializer);
    ServiceFactory Build();
}