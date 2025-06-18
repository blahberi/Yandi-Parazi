namespace Yandi;

public interface IServiceCollection : IList<ServiceDescriptor>
{
    IServiceCollection Finalize();
}
