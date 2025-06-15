using Cornflakes;
using Cornflakes.Extensions;

namespace Test;

class Program
{
    public static void Main(string[] args)
    {
        IServiceProvider serviceProvider = new ServiceProviderBuilder()
            .RegisterTransient<IFoo, Foo>()
            .RegisterSingleton<IBar, Bar>()
            .Build();

        IFoo foo1 = serviceProvider.MustGetService<IFoo>();
        foo1.FooMethod();
        IFoo foo2 = serviceProvider.MustGetService<IFoo>();
        foo2.FooMethod();
    }
}