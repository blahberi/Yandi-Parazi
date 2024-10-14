using System;

namespace Cornflakes
{
    public interface IServiceProvider : IDisposable
    {
        object GetService(Type serviceType);
        IScope CreateScope();

    }
}
