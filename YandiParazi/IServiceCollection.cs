namespace Yandi;

public interface IServiceCollection : IList<ServiceDescriptor>
{
    void Finish();
}
