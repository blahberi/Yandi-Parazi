using Cornflakes;
using Cornflakes.Extensions;
using Cornflakes.Extensions.Lifetimes;

namespace Test;

class Program
{
    public static void Main(string[] args)
    {
        IServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IFoo, Foo>()
            .AddSingleton<IBar, Bar>()
            .AddTransientDecorator<IFoo, FooLoggingDecorator>()
            .AddTransientDecorator<IFoo, AnotherFooDecorator>()
            .BuildProvider();

        IFoo foo = serviceProvider.MustGetService<IFoo>();
        foo.FooMethod();
    }
}