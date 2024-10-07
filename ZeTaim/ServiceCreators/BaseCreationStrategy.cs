using System;
using System.Linq;

namespace ZeTaim
{
    public abstract class BaseCreationStrategy : ICreationStrategy
    {
        public abstract object GetInstance(Type implementationType, IServiceProvider serviceProvider);

        protected object CreateInstance(Type implementationType, IServiceProvider serviceProvider)
        {
            object[] constructorParameters = implementationType.GetConstructors().First()
                .GetParameters()
                .Select(p =>
                    typeof(IServiceProvider)
                    .GetMethod(nameof(IServiceProvider.GetService))
                    .MakeGenericMethod(p.ParameterType)
                    .Invoke(serviceProvider, null))
                .ToArray();
            return Activator.CreateInstance(implementationType, constructorParameters);
        }
    }
}
