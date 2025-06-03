using Cornflakes;
using Cornflakes.Extensions;

namespace Test;

class Program
{
    public static void Main(string[] args)
    {
        IServiceProvider serviceProvider = new ServiceProviderBuilder()
            .RegisterSingleton<IFoo, Foo>()
            .RegisterSingleton<IBar, Bar>()
            .Build();

        IFoo foo = serviceProvider.MustGetService<IFoo>();
        foo.FooMethod();
        foo.OtherFooMethod();

        IBar bar = serviceProvider.MustGetService<IBar>();
        bar.BarMethod();
    }
}