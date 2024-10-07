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

            IFoo foo1 = (IFoo)serviceProvider.GetService(typeof(IFoo));
            IFoo foo2 = (IFoo)serviceProvider.GetService(typeof(IFoo));
            foo1.Method();
            foo2.Method();

            IBar bar = (IBar)serviceProvider.GetService(typeof(IBar));
            bar.Method(7);
        }
    }
}
