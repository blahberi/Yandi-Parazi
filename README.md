# Cornflakes
Cornflakes is a lightweight, *kinda (not really)* blazingly fast Dependency Injection framework for .NET written in C#.
<br>
# Usage
## Defining Services
A **Service Implementation** refers to a class that implements some interface, which we call the **Service Type**. A service implementation can depend on other services by requiring their type through the constructor.

```csharp
// Service Type
internal interface IFoo
{
    void FooMethod();
}

// Another service type
internal interface IBar
{
    int BarMethod();
}

// Service Implementation for type IFoo
internal class Foo : IFoo
{
    private readonly IBar bar;
    public Foo(IBar bar) // Depends on service with type IBar
    {
        this.bar = bar;
        Console.WriteLine($"Foo has been initialized: {this.GetHashCode()}");
    }

    public void FooMethod()
    {
        Console.WriteLine($"Foo method is called: ${this.GetHashCode()}")
        // Calling method of injected service
        int res = this.bar.BarMethod(5);
        Console.WriteLine($"Foo calls Bar which returns {res}: {this.GetHashCode()}");
    }
}
```
In the example above, the service type is `IFoo`, and the implementation is `Foo`. Notice how `Foo` depends on a service with type `IBar`.

## Creation Strategies
**Creation Strategies** define how instances of a service should be created when requested. Cornflakes comes with a couple of built-in creation strategies.

### Transient
Creates a new instance of the service implementation every time it is requested.

### Singleton
Creates only a single instance of the service implementation, and returns that same single instance every time it is requested.

### Scoped
Is similar to the Singleton creation strategy. However, instead of having a single global instance, every **Scope** (which will be discussed later) has its own unique instance.

We can also define custom creation startegies, more on that later.

## Register Services
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

In the example above, we registered the following services:

| Service Type | Service Implementation | Creation Strategy          |
|--------------|------------------------|----------------------------|
| `IFoo`       | `Foo`                  | Singleton                  |
| `IBar`       | `Bar`                  | Transient                  |
| `IBaz`       | `Baz`                  | Scoped                     |
| `IQux`       | `Qux`                  | `MyCustomCreationStrategy` |

## Requesting services
Once our services have been registered through `ServiceProviderBuilder`, we can finalize the service registration process and create the `ServiceProvider` using the `Build()` method.

In order to request a service, we need to specify the **Service Type** of the service we are requesting.

```csharp
// Request the service with type IFoo
IFoo fooService = serviceProvider.GetService<IFoo>()
fooService.FooMethod();
```
Notice how the framework automatically resolves the dependencies of the service implementation.

**note:** The service being requested, all of its dependencies, and all of their dependencies (and so on, recursively), need to be registered. Thus in this example, a service with type `IFoo` needs to be registered. And if the service implemenetation depends on a service with type `IBar`, then it needs to be registered as well.

## Scopes
Cornflakes provides a **Scope** system which allows for more granular control over the lifetime of instances of a service. Within a **Scope**, Scoped services (services that use the Scoped creation strategy) are instantiated once. Each scope has its own single instance of the Scoped service.

### Using Scopes
We can create a scope using the service provider's `CreateScope()` method.

In the following example, assume that the service with type `IBaz` is Scoped.
```csharp
using (IScope scope = serviceProvider.CreateScope()) 
{
    // Get the scope's service provider.
    IServiceProvider scopedProvider = scope.ServiceProvider;

    // Request the service with type IBaz from within the scope.
    IBaz bazService1 = scopedProvider.GetService<IBaz>();
    IBaz bazService2 = scopedProvider.GetService<IBaz>();

    // Since bazService1 and bazService2 are requested within the same scope,
    // they will both point to the same instance (let's call its hash code 'A')
    Console.WriteLine(bazService1.GetHashCode()); // Outputs A
    Console.WriteLine(bazService2.GetHashCode()); // Outputs A
}

using (IScope scope = serviceProvider.CreateScope()) 
{
    // Get the scope's service provider.
    IServiceProvider scopedProvider = scope.ServiceProvider;

    // Request the service with type IBaz from within a different scope.
    IBaz bazService3 = scopedProvider.GetService<IBaz>();
    IBaz bazService4 = scopedProvider.GetService<IBaz>();

    // Since this is a different scope, bazService3 and bazService4 will
    // point to a new instance (with a different hash code, 'B').
    Console.WriteLine(bazService3.GetHashCode()); // Outputs B
    Console.WriteLine(bazService4.GetHashCode()); // Outputs B
}
```

In the example above, we can see that when we request the same Scoped service twice within a scope, we get the same instance. However, if we request that service from a different scope, we get a different instance.

### Nested Scopes
Scoped can be nested. Each scope will have its own unique instances of Scoped Services.

```csharp
using (IScope outerScope = serviceProvider.CreateScope())
{
    IBaz outerBaz = outerScope.ServiceProvider.GetService<IBaz>();

    using (IScope innerScope = outerScope.ServiceProvider.CreateScope())
    {
        IBaz innerBaz = innerScope.ServiceProvider.GetService<IBaz>();

        // outerFoo and innerFoo are different instances
        Console.WriteLine(outerBaz.GetHashCode()); // Outputs A
        Console.WriteLine(innerBaz.GetHashCode()); // Outputs B
    }
}
```

