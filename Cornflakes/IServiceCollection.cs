using System.Collections;
using System.Collections.Generic;

namespace ZeTaim
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        void AddService<TService, TImplementation>() where TImplementation : TService;
    }
}
