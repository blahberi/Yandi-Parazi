namespace Cornflakes.ServiceCreation;

public delegate TService ServiceCreator<out TService>(IServiceProvider serviceProvider);
public delegate void ServiceInitializer(IServiceProvider serviceProvider, object instance);
public delegate IServiceContainer ServiceFactory(IServiceProvider serviceProvider);
public interface IServiceFactoryBuilder<TService>
{
    IServiceFactoryBuilder<TService> Add(ServiceInitializer serviceInitializer);
    ServiceFactory Build();
}