namespace Test;

public class Foo : IFoo
{
    private readonly IBar bar;
    public Foo(IBar bar)
    {
        this.bar = bar;
    }

    public void FooMethod()
    {
        Console.WriteLine($"I am the Foo {this.GetHashCode()}");
        this.bar.BarMethod();
    }
}
