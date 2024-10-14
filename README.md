# Cornflakes
Cornflakes is a lightweight, *kinda (not really)* blazing-fast Dependency Injection framework for .NET written in C#.
<br>
## Usage
### Defining Services
A **Service Implementation** is a class that implement an interface, called the **Service Type** A service implementation can depend on other services by requiring their type through the constructor.

#### Service Type
```csharp
internal interface IFoo
{
    void Method();
}
```
#### Service Implementation
```csharp
internal class Foo : IFoo
{
    private readonly IBar bar;
    public Foo(IBar bar)
    {
        this.bar = bar;
        Console.WriteLine($"Foo has been initialized: {this.GetHashCode()}");
    }

    public void Method()
    {
        Console.WriteLine($"Foo method is called: ${this.GetHashCode()}")
        int res = this.bar.Method(5);
        Console.WriteLine($"Foo calls bar which returns {res}: {this.GetHashCode()}");
    }
}
```
In the example above, the service type is `IFoo`, and the implementation is `Foo`. Notice how `Foo` depends on a service with type `IBar`.

### Register Services
Service registration is done through the `ServiceProviderBuilder`.

```csharp
IServiceProvider serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>()
    .RegisterTransient<IBar, Bar>()
    .RegisterScoped<IBaz, Baz>()
    .RegisterService<IQux, Qux>(new MyCustomCreationStrategy())
    .Build();
```
When registering a service, we specify the **Service Type**, **Service Implementation**, and **Creation Strategy**.

**Creation Strategies** define how a service should be created when requested. Creation strategies implement `ICreationStrategy`. Cornflakes comes with a couple of built-in creation strategies.

#### Transient
The **Transient** creation strategy, creates a new instance of the service implementation every time it is requested.

#### Singleton
The **Singleton** creation strategy, creates only a single instance of the service implementation, and returns that same single instance every time it is requested.

#### Scoped
The **Scoped** creation strategy, is similar to the Singleton creation strategy. However, instead of having a single global instance, every `Scope` (which will be discussed later) has its own unique instance.

In the example above, we registered the following services:

| Service Type | Service Implementation | Creation Strategy          |
|--------------|------------------------|----------------------------|
| `IFoo`       | `Foo`                  | Singleton                  |
| `IBar`       | `Bar`                  | Transient                  |
| `IBaz`       | `Baz`                  | Scoped                     |
| `IQux`       | `Qux`                  | `MyCustomCreationStrategy` |

Notice how we can use custom creation strategies. More on defining custom creation strategies later.


### Requesting services
Once our services have been registered through `ServiceProviderBuilder`, we can create the `ServiceProvider` using the `Build()` method.

In order to request a service, we need to specify the **Service Type** of the service we are requesting.

```csharp
IFoo fooService = serviceProvider.GetService<IFoo>()
```
**note:** The service being requested, all of its dependencies, and all of their dependencies (and so on, recursively), need to be registered. Thus in this example, a service with type `IFoo` needs to be registered. And if the service implemenetation depends on a service with type `IBar`, then it needs to be registered as well.