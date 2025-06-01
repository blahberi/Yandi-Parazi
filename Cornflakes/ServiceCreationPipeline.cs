namespace Cornflakes;

public delegate object ServiceFactory(IServiceProvider serviceProvider);
public delegate object ServiceFactoryWrapper(IServiceProvider serviceProvider, object instance);
public delegate void OnInitialized(IServiceProvider serviceProvider, object instance);

internal class ServiceCreationPipeline : IServiceCreationPipeline, IServiceCreationPipelineBuilder
{
    private ServiceFactory serviceFactory;
    private OnInitialized? onInitialized;
    
    public ServiceCreationPipeline(ServiceFactory serviceFactory)
    {
        this.serviceFactory = serviceFactory;
    }
    public void Invoke(IServiceProvider serviceProvider, out object instance)
    {
        instance = this.serviceFactory(serviceProvider);
        this.onInitialized?.Invoke(serviceProvider, instance);
    }

    public IServiceCreationPipelineBuilder Add(ServiceFactoryWrapper serviceProviderWrapper)
    {
        ServiceFactory currentFactory = this.serviceFactory;
        this.serviceFactory = serviceProvider => 
            serviceProviderWrapper(serviceProvider, currentFactory(serviceProvider));
        return this;
    }
    
    public IServiceCreationPipelineBuilder Add(OnInitialized onInitialized)
    {
        this.onInitialized += onInitialized;
        return this;
    }
    
    public IServiceCreationPipeline Build()
    {
        return this;
    }
}

public static class ServiceCreationPipelineBuilder
{
    public static IServiceCreationPipelineBuilder Create(ServiceFactory serviceFactory)
    {
        return new ServiceCreationPipeline(serviceFactory);
    }
    
    public static IServiceCreationPipelineBuilder ToPipeline(this ServiceFactory serviceFactory)
    {
        return Create(serviceFactory);
    }
}