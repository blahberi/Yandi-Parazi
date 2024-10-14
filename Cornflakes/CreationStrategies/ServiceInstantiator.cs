using System;
using System.Linq;

namespace Cornflakes.CreationStrategies
{
    internal static class ServiceInstantiator
    {
        public static object CreateInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            object[] constructorParameters = implementationType.GetConstructors().First()
                .GetParameters()
                .Select(p => serviceProvider.GetService(p.ParameterType))
                .ToArray();
            return Activator.CreateInstance(implementationType, constructorParameters);
        }
    }
}
