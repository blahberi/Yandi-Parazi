using Yandi;
using Yandi.Extensions;
using Yandi.Extensions.Lifetimes;

namespace Test;

class Program
{
    public static void Main(string[] args)
    {
        IServiceProvider serviceProvider = new ServiceCollection()
            .AddTransient<IFoo, Foo>()
            .AddSingleInstance<IBar, Bar>()
            .AddTransientDecorator<IFoo, FooLoggingDecorator>()
            .AddTransientDecorator<IFoo, AnotherFooDecorator>()
            .BuildProvider();

        IFoo foo = serviceProvider.MustGetService<IFoo>();
        foo.FooMethod();

        Console.ReadLine();
    }
}
