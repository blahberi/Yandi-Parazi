using System;

namespace ZeTaim
{
    public interface IServiceProviderBuilder
    {
        IServiceProviderBuilder RegisterService<TService, TImplementation>() where TImplementation : TService;

        IServiceProviderBuilder RegisterServices(IServiceCollection services);

        IServiceProviderBuilder RegisterServiceCreator<TAttribute, TCreator>() 
            where TAttribute : Attribute 
            where TCreator : ICreationStrategy;

        IServiceProvider Build();
    }
}
