using Cornflakes.Extensions;

namespace Cornflakes.ServiceCreation;

public delegate object DecoratorCreator(IServiceProvider serviceProvider, object instance);
public static class Decorators
{
    internal static ServiceFactory GetDecoratedServiceFactory<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        ServiceDescriptor descriptor = collection.FindService<TService>();
        DecoratorCreator decoratorCreator = DependencyResolver.GetDecoratorCreator<TService, TDecorator>();
        ServiceInitializer? decoratorInjector = DependencyResolver.TryGetMemberInjector<TDecorator>();
        return serviceProvider =>
        {
            IServiceContainer innerContainer = descriptor.LifetimeManager.GetInstance(serviceProvider);
            object innerService = innerContainer.GetService(serviceProvider);
            object decoratorInstance = decoratorCreator(serviceProvider, innerService);
            List<ServiceInitializer> onInitialized = [];
            if (decoratorInjector != null)
            {
                onInitialized.Add(decoratorInjector);
            }
            onInitialized.Add((sp, _) => innerContainer.GetService(sp));
            return new ServiceContainer(decoratorInstance, onInitialized);
        };
    }
}