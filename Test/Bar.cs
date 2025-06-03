using Cornflakes;

namespace Test;

public class Bar : IBar
{
    [Inject]
    private readonly IFoo foo;
    
    public void BarMethod()
    {
        Console.WriteLine("I am the Bar");
        this.foo.OtherFooMethod();
    }
}