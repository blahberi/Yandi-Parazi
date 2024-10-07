using ZeTaim;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = new ServiceProviderBuilder()
                .RegisterService<IFoo, Foo>()
                .RegisterService<IBar, Bar>()
                .Build();

            IFoo foo1 = serviceProvider.GetService<IFoo>();
            IFoo foo2 = serviceProvider.GetService<IFoo>();
            foo1.Method();
            foo2.Method();

            IBar bar = serviceProvider.GetService<IBar>();
            bar.Method(7);
        }
    }
}