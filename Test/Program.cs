using Cornflakes;
using System;
using IServiceProvider = Cornflakes.IServiceProvider;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = new ServiceProviderBuilder()
                .RegisterSingleton<IFoo, Foo>()
                .RegisterTransient<IBar, Bar>()
                .RegisterScoped<IBaz, Baz>()
                .Build();
        }
    }
}