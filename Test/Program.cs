using Cornflakes;

namespace Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Create the service provider using the builder
            Cornflakes.IServiceProvider serviceProvider = new ServiceProviderBuilder()
                .RegisterSingleton<IFoo, Foo>()
                .RegisterTransient<IBar, Bar>()
                .RegisterScoped<IBaz, Baz>()
                .Build();
        }
    }
}