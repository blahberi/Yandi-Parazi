# Cornflakes
Cornflakes is a lightweight, blazingly fast and highly extendable Dependency Injection framework for .NET written in C#.
<br>
# Usage
## Defining Services
A **Service** refers to an interface which is implemented by a class, which we call the **Service Implementation**. A service implementation can depend on other services by requiring their type through the constructor.

```csharp
// Service
interface IFoo
{
    void FooMethod();
}

// Another service
interface IBar
{
    int BarMethod();
}

// Service Implementation for IFoo
class Foo : IFoo
{
    private readonly IBar bar;
    public Foo(IBar bar) // Depends on service IBar
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
In the example above, the service is `IFoo`, and the implementation is `Foo`, which depends on the service `IBar`.

## Lifetime Manager
**Lifetime Managers** define how instances of a service should be created when requested. Cornflakes comes with a couple of built-in lifetime managers.

### Transient
Creates a new instance of the service every time it is requested.

### Singleton
Creates only a single instance of the service, and returns that same single instance every time it is requested.

### Scoped
Is similar to the Singleton lifetime manager. However, instead of having a single global instance, every **Scope** (which will be discussed later) has its own unique instance.

We can also define custom creation startegies, more on that later.

## Register Services
Service registration is done through the `ServiceProviderBuilder`.

```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>()
    .RegisterTransient<IBar, Bar>()
    .RegisterScoped<IBaz, Baz>()
    .Build();
```
When registering a service, we specify the **Service**, **Service Implementation**, and **lifetime Strategy**.

In the example above, we registered the following services:

| Service | Service Implementation | Lifetime Manager           |
|---------|------------------------|----------------------------|
| `IFoo`  | `Foo`                  | Singleton                  |
| `IBar`  | `Bar`                  | Transient                  |
| `IBaz`  | `Baz`                  | Scoped                     |

## Requesting services
Once our services have been registered through `ServiceProviderBuilder`, we can finalize the service registration process and create the `ServiceProvider` using the `Build()` method.

In order to request a service, we need to specify the the service we are requesting.

```csharp
// Request the service with type IFoo
IFoo fooService = serviceProvider.GetService<IFoo>()
fooService.FooMethod();
```
Notice how the framework automatically resolves the dependencies of the service implementation.

> [!IMPORTANT]
> The service being requested, all of its dependencies, and all of their dependencies (and so on, recursively), need to be registered. Thus in this example, the service with `IFoo` needs to be registered. And if the service implemenetation depends on the service `IBar`, then it needs to be registered as well.

## Scopes
Cornflakes provides a **Scope** system which allows for more granular control over the lifetime of instances of a service. Within a **Scope**, Scoped services (services that use the Scoped lifetime manager) are instantiated once. Each scope has its own single instance of the Scoped service.

### Using Scopes
We can create a scope using the service provider's `CreateScope()` method.

In the following example, assume that the service `IFoo` is Scoped.
```csharp
using (IScope scope = serviceProvider.CreateScope()) 
{
    // Get the scope's service provider.
    IProviderOfServices scopedProvider = scope.ServiceProvider;

    // Request the service IFoo from within the scope.
    IFoo fooService1 = scopedProvider.GetService<IFoo>();
    IFoo fooService2 = scopedProvider.GetService<IFoo>();

    // Since fooService1 and fooService2 are requested within the same scope,
    // they will both point to the same instance (let's call its hash code 'A')
    Console.WriteLine(fooService1.GetHashCode()); // Outputs A
    Console.WriteLine(fooService2.GetHashCode()); // Outputs A
}

using (IScope scope = serviceProvider.CreateScope()) 
{
    // Get the scope's service provider.
    IProviderOfServices scopedProvider = scope.ServiceProvider;

    // Request the service IFoo from within a different scope.
    IFoo fooService3 = scopedProvider.GetService<IFoo>();
    IFoo fooService4 = scopedProvider.GetService<IFoo>();

    // Since this is a different scope, fooService3 and fooService4 will
    // point to a new instance (with a different hash code, 'B').
    Console.WriteLine(fooService3.GetHashCode()); // Outputs B
    Console.WriteLine(fooService4.GetHashCode()); // Outputs B
}
```

In the example above, we can see that when we request the same Scoped service twice within a scope, we get the same instance. However, if we request that service from a different scope, we get a different instance.

### Nested Scopes
Scoped can be nested. Each scope will have its own unique instances of Scoped Services.

```csharp
using (IScope outerScope = serviceProvider.CreateScope())
{
    Foo outerFoo = outerScope.ServiceProvider.GetService<IFoo>();

    using (IScope innerScope = outerScope.ServiceProvider.CreateScope())
    {
        IFoo innerFoo = innerScope.ServiceProvider.GetService<IFoo>();

        // outerFoo and innerFoo are different instances
        Console.WriteLine(outerFoo.GetHashCode()); // Outputs A
        Console.WriteLine(innerFoo.GetHashCode()); // Outputs B
    }
}
```

## Member Injection
Cornflakes allows for injecting dependnecies through members, instead of the constructor. This is useful for legacy codebases that have *circular dependencies*.

For example, let us assume that `Foo` depends on `IBar` and `Bar` depends on `IFoo`. We can resolve this situation using **Member Injection**.
We can use the `[Inject]` attribute to mark fields and properties that we want to inject dependencies through them.

```csharp
class Foo : IFoo {
    [Inject]
    private readonly IBar bar;
    // Foo implementation ...
}

class Bar : IBar {
    [Inject]
    private readonly IFoo foo;
    // Bar implementation
}
```
> [!WARNING]
> Circular dependencies are a bad habbit. This feature is targeted at large legacy codebases that already have circular dependencies rooted deep in them. It is highly recommended you don't rely on this feature, and instead plan clean modular architectures with no circular dependencies.
*Member injection displays warnings as they are highly unrecommmended*

## Service Factory Functions 
**Service Factory Functions** are functions that create a new instance of a service. They are called by the lifetime manager whenever it creates a new instance of a service.

### Custom Service Factory Functions
We can define custom factory functions, which can be useful when we need to perform some custom logic when creating a service instance. Custom factory functions are defined when registering a service, and they are defined as delegates that take an `IProviderOfServices` as an argument and return an instance of the service.

```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>() // Default factory function
    .RegisterTransient<IBar>(sp => {
        // Custom factory function
        return new Bar();
    }) 
    .RegisterScoped<IBaz, Baz>()
    .Build();
```

If a service implementation depends on another service and we are using a custom factory function for that service, we have to resolve the dependencies ourselves within the factory function.
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterTransient<IBar>(sp => {
        // Custom factory function
        IFoo foo = sp.GetService<IFoo>();
        return new Bar(foo); // Resolve the dependency (Bar depends on IFoo)
    }) 
    .RegisterScoped<IBaz, Baz>()
    .Build();
```

Service factory functions do not support member injection. Use the `AttachMemberInjection<TImplementation>()` method to add member injection support.
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterTransient<IBar>(((sp => {
        // Custom factory function
        IFoo foo = sp.GetService<IFoo>();
        return new Bar(foo); // Resolve the dependency (Bar depends on IFoo)
    }).AttachMemberInjection<Bar>()) // add member injection
    .RegisterScoped<IBaz, Baz>()
    .Build();
```

### Default Service Factory Function
The default service factory function simply creates a new instance of the service and resolves its dependencies through the constructor and through member injection as discussed earlier. Cornflakes will use the default service factory function if a custom factory function is not provided when registering a service.

Cornflakes uses the JIT to dynamically generate and compile the default service factory function, which provides high performance service instantiation and dependency resolution.
For example, if we have the follwing service registration:
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>() // Assume Foo depends on IBar and IBaz
    .RegisterTransient<IBar, Bar>() // Assume Bar depends on IBaz
    .RegisterScoped<IBaz, Baz>() // Assume Baz has no dependencies
    .Build();
```
Then the default service loader functions generated by Cornflakes would look like this:

```csharp
// IFoo Default Service Factory Function
((IProviderOfServices serviceProvider) => {
    return new Foo(serviceProvider.GetService<IBar>(), serviceProvider.GetService<IBaz>());
}).AttachMemberInjection<Foo>()

// IBar Default Service Factory Function
((IProviderOfServices serviceProvider) => {
    return new Bar(serviceProvider.GetService<IBaz>());
}).AttachMemberInjection<Bar>()

// IBaz Default Service Factory Function
(IProviderOfServices serviceProvider) => {
    return new Baz();
}).AttachMemberInjection<Baz>()
```

the `AttachMemberInjection<TImplementation>()` method uses IL emission to generate blazingly fast member injection functions.


The default service factory function can be retrieved by using the `DependencyResolver.GetServiceFactory<TImplementation>()` method where `TImplementation` is the service implementation.
We can even use the default service factory function as a custom factory function. In fact,
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>() // Default factory function
    .Build();
```
is completely equivalent to
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    // Passing the default factory function as a custom factory function
    .RegisterSingleton<IFoo>(DependencyResolver.GetServiceFactory<Foo>().AttachMemberInjection<Foo>())
    .Build();
```
Although the former is cleaner, less verbose, and more concise than the latter, which is why it is recommended.

## Custom Lifetime Managers
Custom lifetime managers can be useful when we need to perform some custom logic for handling the lifetime of service instances. We can define custom lifetime managers by implementing the `ILifetimeManager` interface. As an example, we will implement the `Transient` and `Singleton` lifetime managers. For the sake of simplicity, we will not implement thread-safty. However, rest assured, all the build-in lifetime managers are fully thread safe.

### Transient lifetime Manager Implementation
```csharp
class CustomTransientLifetime: ILifetimeManager
{
    private readonly ServiceFactory serviceFactory;

    public CustomTransientLifetime(ServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    public object GetInstance(IProviderOfServices serviceProvider)
    {
        // Custom logic to create a service instance
        return this.serviceFactory(serviceProvider);
    }
}
```
It is good practice to decouple the instantitation logic from the lifetime manager by requiring a `ServiceFactory` delegate in the constructor, which allows for custom service factory functions.

Now we can register services using our custom lifetime manager. For the examples below, we will assume that `Bar` depends on `IFoo`.
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterService<IFoo, Foo>(new CustomTransientLifetime(
        DefaultServiceFactory.GetServiceFactory<Foo>())) // Default factory function
    .RegisterService<IBar, Bar>(new CustomTransientLifetime((serviceProvider) => {
        // Custom factory function
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    }))
    .Build();
```

### Singleton lifetime Strategy Implementation
```csharp
class CustomSingletonLifetime: ILifetimeManager
{
    private readonly ServiceFactory serviceFactory;
    private object instance;
    public CustomSingletonLifetime(ServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    public object GetInstance(IProviderOfServices serviceProvider)
    {
        if (this.instance == null)
        {
            this.instance = this.serviceFactory(serviceProvider);
        }
        return this.instance;
    }
}
```
Then service registation is performed as follows:
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterService<IFoo>(new CustomSingletonLifetime(
        DefaultServiceFactory.GetServiceFactory<Foo>())) // Default factory function
    .RegisterService<IBar>(new CustomSingletonLifetime((serviceProvider) => {
        // Custom factory function
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    }))
    .Build();
```

### Extending the Builder
Notice that currently, registering services with our custom lifetime managers is a bit verbose, unlike the built-in lifetime managers. We can extend the `ServiceProviderBuilder` to provide a more fluent API for registering services with custom lifetime managers.

```csharp
public static class CustomServiceProvideBuilderExtensions
{
    public static IServiceProviderBuilder RegisterCustomTransient<TService>(
        this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
    {
        return builder.RegisterService<TService>(new CustomTransientLifetime(serviceFactory));
    }

    public static IServiceProviderBuilder RegisterCustomSingleton<TService>(
        this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
    {
        return builder.RegisterService<TService>(new CustomSingletonLifetime(serviceFactory));
    }

    // Extension methods for using the default service factory function
    public static IServiceProviderBuilder RegisterCustomTransient<TService, TImplementation>(
        this IServiceProviderBuilder builder)
    {
        return builder.RegisterService<TService>(new CustomTransientLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>()));
    }

    public static IServiceProviderBuilder RegisterCustomSingleton<TService, TImplementation>(
        this IServiceProviderBuilder builder)
    {
        return builder.RegisterService<TService>(new CustomSingletonLifetime(
            DependencyResolver.GetServiceFactory<TImplementation>()));
    }
}
```
Now the service registration process is much more fluent and streamlined, and is comparable to the built-in lifetime managers.
```csharp
IProviderOfServices serviceProvider = new ServiceProviderBuilder()
    .RegisterCustomSingleton<IFoo, Foo>() // Default factory function
    .RegisterCustomTransient<IBar>((serviceProvider) => {
        // Custom factory function
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    })
    .Build();
```
