using System;

namespace ZeTaim
{
    public interface IServiceProvider
    {
        TService GetService<TService>();
    }
}
