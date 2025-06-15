using Cornflakes;
using Cornflakes.Extensions;

namespace Test;

class Program
{
    public static void Main(string[] args)
    {
        IServiceProvider serviceProvider = new ServiceProviderBuilder()
            .RegisterScoped<IFoo, Foo>()
            .RegisterSingleton<IBar, Bar>()
            .Build();

        serviceProvider.MustGetService<IFoo>().FooMethod();
        IServiceProvider temp;
        using (IScope scope = serviceProvider.CreateScope())
        {
            temp = scope.ServiceProvider;
            scope.ServiceProvider.MustGetService<IFoo>().FooMethod();
            scope.ServiceProvider.MustGetService<IFoo>().FooMethod();
        }
        temp.MustGetService<IFoo>().FooMethod();
    }
}