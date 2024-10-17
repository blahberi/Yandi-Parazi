using Cornflakes;
using System;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cornflakes.IServiceProvider serviceProvider = new ServiceProviderBuilder()
                .RegisterSingleton<IFoo, Foo>()
                .RegisterTransient<IBar, Bar>()
                .RegisterScoped<IBaz, Baz>()
                .Build();

            using (IScope scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetService<IBaz>();
            }

            Console.ReadLine();
        }
    }
}