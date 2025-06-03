using Cornflakes;

namespace Test;

public class Foo : IFoo
{
    [Inject]
    private readonly IBar bar;

    public void FooMethod()
    {
        Console.WriteLine("I am the Foo");
        this.bar.BarMethod();
    }
    
    public void OtherFooMethod()
    {
        Console.WriteLine("I am the Foo (the second time)");
    }
}