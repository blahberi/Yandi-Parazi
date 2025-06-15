namespace Cornflakes;

public interface IServiceCollection : IList<ServiceDescriptor>
{
    IServiceCollection Finalize();
}
