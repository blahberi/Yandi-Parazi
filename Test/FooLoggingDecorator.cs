namespace Test;

public class FooLoggingDecorator : IFoo
{
    private readonly IFoo foo;

    public FooLoggingDecorator(IFoo foo)
    {
        this.foo = foo;
    }

    public void FooMethod()
    {
        Console.WriteLine($"Before calling FooMethod on {this.foo.GetHashCode()}");
        this.foo.FooMethod();
        Console.WriteLine($"After calling FooMethod on {this.foo.GetHashCode()}");
    }
}