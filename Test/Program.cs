using Cornflakes;

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

            IFoo foo = serviceProvider.GetService<IFoo>();
        }
    }
}