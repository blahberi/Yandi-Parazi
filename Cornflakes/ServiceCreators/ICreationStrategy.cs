using System;

namespace Cornflakes
{
    public interface ICreationStrategy
    {
        object GetInstance(Type implementationType, IServiceProvider serviceProvider);
    }
}
