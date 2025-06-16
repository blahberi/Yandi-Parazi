using Cornflakes;
using Cornflakes.Extensions;

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
            .BuildServiceProvider();

        IFoo foo1 = serviceProvider.MustGetService<IFoo>();
        foo1.FooMethod();
    }
}