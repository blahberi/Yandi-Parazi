using System;

namespace ZeTaim
{
    public interface ICreationStrategy
    {
        object GetInstance(Type implementationType, IServiceProvider serviceProvider);
    }
}
