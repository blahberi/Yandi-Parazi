using System;
using System.Linq;

namespace Cornflakes
{
    public abstract class BaseCreationStrategy : ICreationStrategy
    {
        public abstract object GetInstance(Type implementationType, IServiceProvider serviceProvider);

        protected object CreateInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            object[] constructorParameters = implementationType.GetConstructors().First()
                .GetParameters()
                .Select(p => serviceProvider.GetService(p.ParameterType))
                .ToArray();
            return Activator.CreateInstance(implementationType, constructorParameters);
        }
    }
}
