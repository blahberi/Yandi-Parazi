using System;

namespace ZeTaim
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType);
    }
}
