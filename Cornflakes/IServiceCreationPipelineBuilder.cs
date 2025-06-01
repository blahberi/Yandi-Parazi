namespace Cornflakes;

public interface IServiceCreationPipelineBuilder
{
    IServiceCreationPipelineBuilder Add(ServiceFactoryWrapper serviceProviderWrapper);
    IServiceCreationPipelineBuilder Add(OnInitialized onInitialized);
    IServiceCreationPipeline Build();
}