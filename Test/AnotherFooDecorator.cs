namespace Test;

public class AnotherFooDecorator : IFoo
{
    private readonly IFoo foo;

    public AnotherFooDecorator(IFoo foo)
    {
        this.foo = foo;
    }

    public void FooMethod()
    {
        Console.WriteLine($"Another decorator before calling FooMethod on {this.foo.GetHashCode()}");
        this.foo.FooMethod();
        Console.WriteLine($"Another decorator after calling FooMethod on {this.foo.GetHashCode()}");
    }
}