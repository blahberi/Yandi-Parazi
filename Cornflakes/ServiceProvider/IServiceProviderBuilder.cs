namespace Cornflakes;

public interface IServiceProviderBuilder
{
    IServiceProviderBuilder RegisterService(ServiceDescriptor serviceDescriptor); 
    IServiceProvider Build();
}
