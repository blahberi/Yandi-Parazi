# Cornflakes
Cornflakes is Dependency Injection framework for .NET written in C# that is lightweight and fast, yet abstract and easy to extend.
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

We can also define custom lifetime managers, more on that later.

## Register Services
Service registration is done through the `ServiceCollection`.

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IFoo, Foo>()
    .AddTransient<IBar, Bar>()
    .AddScoped<IBaz, Baz>()
    .BuildProvider();
```

When registering a service, we specify the **Service**, **Service Implementation**, and **lifetime Strategy**.

In the example above, we registered the following services:

| Service | Service Implementation | Lifetime Manager |
| ------- | ---------------------- | ---------------- |
| `IFoo`  | `Foo`                  | Singleton        |
| `IBar`  | `Bar`                  | Transient        |
| `IBaz`  | `Baz`                  | Scoped           |

## Requesting services
Once our services have been registered through `ServiceCollection`, we can finalize the service registration process and create the `ServiceProvider` using the `BuildProvider()` method.

In order to request a service, we need to specify the the service we are requesting.

```csharp
// Request the service with type IFoo
IFoo fooService = serviceProvider.GetService<IFoo>()
fooService.FooMethod();
```

> [!NOTE]
>Notice how the framework automatically resolves the dependencies of the service implementation.

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
    IServiceProvider scopedProvider = scope.ServiceProvider;

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
    IServiceProvider scopedProvider = scope.ServiceProvider;

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

## Service Factories
### Service Factories
Cornflakes allows you to provide **Service Factory Functions** which can be useful when we need to perform some custom logic when creating a service instance. Custom service factories are defined when registering a service, and they are defined as delegates that take an `IServiceProvider` as an argument and return an instance of the service as `object`.

```csharp
object ServiceFactory(IServiceProvider serviceProvider)
```

Here is an example,

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IFoo, Foo>() // Default service factory
    .AddTransient<IBar>(sp => {
        // Custom service factory
        return new Bar();
    }) 
    .BuildProvider();
```

If a service implementation depends on another service and we are using a custom service factory for that service, we have to resolve the dependencies ourselves within the factory function.

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IFoo, Foo>() // Default service factory
    .AddTransient<IBar>(sp => {
        // Custom service factory
        IFoo foo = sp.GetService<IFoo>();
        return new Bar(foo); // Resolve the dependency (Bar depends on IFoo)
    }) 
    .BuildProvider();
```

### Default Service Factory
The default service factory simply creates a new instance of the service and resolves its dependencies through the constructor as discussed earlier. Cornflakes will use the default service factory if a custom service factory is not provided when registering a service.

Cornflakes uses the JIT to dynamically generate and compile the default service factory, which provides high performance service instantiation and dependency resolution.
For example, if we have the follwing service registration:

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IFoo, Foo>() // Assume Foo depends on IBar and IBaz
    .AddTransient<IBar, Bar>() // Assume Bar depends on IBaz
    .AddScoped<IBaz, Baz>() // Assume Baz has no dependencies
    .BuildProvider();
```

Then the default service loader functions generated by Cornflakes would look like this:

```csharp
// IFoo Default Service Factory
((IServiceProvider serviceProvider) => {
    return new Foo(serviceProvider.GetService<IBar>(), serviceProvider.GetService<IBaz>());
})

// IBar Default Service Factory
((IServiceProvider serviceProvider) => {
    return new Bar(serviceProvider.GetService<IBaz>());
})

// IBaz Default Service Factory
(IServiceProvider serviceProvider) => {
    return new Baz();
})
```

The default service factory function can be retrieved by using the `DependencyResolver.GetServiceFactory<TImplementation>()` method where `TImplementation` is the service implementation. For example,

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    // Passing the default factory function as a custom factory function
    .AddSingleton<IFoo>(DependencyResolver.GetServiceFactory<Foo>())
    .BuildProvider();
```

## Decorators
Cornflakes allows for registering **Decorator Services**. A decorator service is a service that implements some `TService` but also directly depends on `TService`. For example, here is a logging decorator for `IFoo`, `FooLoggingDecorator`:

```csharp
class FooLoggingDecorator : IFoo
{
    private readonly IFoo foo;
    private readonly ILogger logger;

    public FooLoggingDecorator(IFoo foo, ILogger logger) 
    {
        this.foo = foo;
    }

    public FooMethod() 
    {
        this.logger.Log("calling FooMethod");
        this.foo.FooMethod();
        this.logger.Log("FooMethod called");
    }
}
```

We can now register this decorator like so:

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<IFoo, Foo>()
    .AddSingleton<ILogger, Logger>()
    .AddTransientDecorator<IFoo, FooLoggingDecorator>()
    .BuildProvider()
```

> [!NOTE]
> - Notice how the decorator can also have dependencies and the framework will automatically resolve them the same way it resolves dependencies for regular services.
> - Also notice that decorators can have their own lifetime. In the above example we are registering a singleton service with a transient decorator. This means, that everytime we request `IFoo`, we will get a new instance `FooLoggingDecorator` that has the same instance of `Foo`


# Extending
## Custom Lifetime Managers
Custom lifetime managers can be useful when we need to perform some custom logic for handling the lifetime of service instances. We can define custom lifetime managers by implementing the `ILifetimeManager` interface. For this guide, we will implement `CustomTransientLifetime` which is identical to the `Transient` lifetime manager.

```csharp
class CustomTransientLifetime: ILifetimeManager
{
    private readonly IServiceFactory serviceFactory;

    public CustomTransientLifetime(IServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    public IServiceContainer GetInstance(IServiceProvider serviceProvider)
    {
        return this.serviceFactory.Create(serviceProvider);
    }
}
```

It is good practice to decouple the instantiation logic from the lifetime manager by requiring a `ServiceFactory` delegate in the constructor, which allows for custom service factory functions.

Now we can register services using our custom lifetime manager. For the examples below, we will assume that `Bar` depends on `IFoo`.

```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddService<IFoo, Foo>(new CustomTransientLifetime(
        DependencyResolver.GetServiceFactory<Foo>())
    )
    .AddService<IBar>(new CustomTransientLifetime(serviceProvider => {
        // Custom service factory
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    }))
    .BuildProvider();
```

### Extending the registration
Notice that currently, registering services with our custom lifetime managers is a bit verbose, unlike the built-in lifetime managers. We can extend `IServiceCollection` to provide a more fluent API for registering services with custom lifetime managers.

`IServiceCollection` already comes with extension methods for handling service registartion with custom lifetime managers. These extension methods rely on `LifetimeManagerFactory`, a delegate that receives a service factory and returns a lifetime manager.

```csharp
ILifetimeManager LifetimeManagerFactory(ServiceFactory serviceFactory)
```

So we can add extension methods for our `CustomTransientLifetime` like so:

```csharp
public static class CustomTransientLifetimeExtensions
{
    private static readonly LifetimeManagerFactory LifetimeManagerFactory = sf => new CustomTransientLifetime(sf);
    public static IServiceCollection AddCustomTransient<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
    {
        return collection.AddService<TService>(new TransientLifetime(serviceFactory));
    }
    
    public static IServiceCollection AddCustomTransient<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService, TImplementation>(LifetimeManagerFactory);
    }

    public static IServiceCollection AddCustomTransientDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        return collection.Decorate<TService, TDecorator>(LifetimeManagerFactory);
    }
}
```

And now registering services with our custom lifetime manager becomes as fluent as the built-in registration.

```csharp
IServiceCollection serviceProvider = new ServiceCollection()
    .AddCustomTransient<IFoo, Foo>() // Default factory function
    .AddCustomTransient<IBar>(serviceProvider => {
        // Custom factory function
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    })
    .BuildProvider();
```

### Full examlpe
As a full example for adding custom lifetime managers, we will now implement `CustomSingletonLifetime` which works like the `Singleton` lifetime manager. For the sake of simplicity, we will not implement thread-safty. However, rest assured, all the build-in lifetime managers are fully thread safe.

```csharp
class CustomSingletonLifetime: ILifetimeManager
{
    private readonly IServiceFactory serviceFactory;
    private object instance;
    public CustomSingletonLifetime(ServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    public object GetInstance(IServiceProvider serviceProvider)
    {
        if (this.instance == null)
        {
            this.instance = this.serviceFactory.Create(serviceProvider);
        }
        return this.instance;
    }
}

public static class CustomSingletonLifetimeExtensions
{
    private static readonly LifetimeManagerFactory LifetimeManagerFactory = sf => new CustomSingletonLifetime(sf);
    public static IServiceCollection AddCustomSingleton<TService>(this IServiceCollection collection, ServiceFactory serviceFactory)
    {
        return collection.AddService<TService>(new TransientLifetime(serviceFactory));
    }
    
    public static IServiceCollection AddCustomSingleton<TService, TImplementation>(this IServiceCollection collection)
        where TService : class
        where TImplementation : class, TService
    {
        return collection.AddService<TService, TImplementation>(LifetimeManagerFactory);
    }

    public static IServiceCollection AddCustomSingletonDecorator<TService, TDecorator>(this IServiceCollection collection)
        where TService : class
        where TDecorator : class, TService
    {
        return collection.Decorate<TService, TDecorator>(LifetimeManagerFactory);
    }
}
```

# Benchmarks
| Framework      | Mean     | Error    | StdDev   | Median   | Gen0   | Allocated |
|--------------- |---------:|---------:|---------:|---------:|-------:|----------:|
| Cornflakes     | 17.52 us | 0.509 us | 1.501 us | 18.10 us |      - |      24 B |
| MSDI           | 17.14 us | 0.371 us | 1.094 us | 17.63 us |      - |      24 B |
| Autofac        | 17.37 us | 0.363 us | 1.070 us | 17.83 us | 0.1526 |    1312 B |
| SimpleInjector | 17.28 us | 0.568 us | 1.673 us | 17.73 us |      - |      24 B |
