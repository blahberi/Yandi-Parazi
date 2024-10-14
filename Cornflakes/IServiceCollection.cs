using System.Collections;
using System.Collections.Generic;

namespace Cornflakes
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        void AddService<TService, TImplementation>(ICreationStrategy creationStrategy) where TImplementation : TService;
    }
}
