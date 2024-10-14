using System;
using System.Linq;

namespace Cornflakes.CreationStrategies
{
    public delegate object ServiceFactory(IServiceProvider serviceProvider);
    public static class DefaultServiceFactory
    {
        public static ServiceFactory GetServiceFactory<TImplementation>()
        {
            return (IServiceProvider serviceProvider) => ServiceInstantiator.CreateInstance(typeof(TImplementation), serviceProvider);
        }
    }
}
