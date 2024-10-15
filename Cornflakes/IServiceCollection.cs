using Cornflakes.LifetimeStrategies;
using System.Collections.Generic;

namespace Cornflakes
{
    public interface IServiceCollection : IList<ServiceDescriptor>
    {
        IServiceCollection AddService<TService, TImplementation>(ILfetimeStrategy creationStrategy) where TImplementation : TService;
    }
}
