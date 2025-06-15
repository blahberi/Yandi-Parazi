namespace Cornflakes.ServiceCreation;

public delegate TService ServiceCreator<out TService>(IServiceProvider serviceProvider);
public delegate TService ServiceCreationWrapper<TService>(IServiceProvider serviceProvider, TService instance);
public delegate void ServiceInitializer(IServiceProvider serviceProvider, object instance);
public interface IServiceFactoryBuilder<TService>
{
    IServiceFactoryBuilder<TService> Add(ServiceCreationWrapper<TService> serviceProviderWrapper);
    IServiceFactoryBuilder<TService> Add(ServiceInitializer serviceInitializer);
    IServiceFactory Build();
}