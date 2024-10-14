using System;

namespace Cornflakes
{
    public interface IServiceProvider
    {
        IScope Scope { get; }
        object GetService(Type serviceType);
        IScope CreateChildScope();

    }
}
