using Cornflakes.CreationStrategies;
using System.Collections.Generic;

namespace Cornflakes
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        IServiceCollection AddService<TService, TImplementation>(ICreationStrategy creationStrategy) where TImplementation : TService;
    }
}
