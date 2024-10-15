# Cornflakes
Cornflakes is a lightweight, blazingly fast Dependency Injection framework for .NET written in C#.
<br>
# Usage
## Defining Services
A **Service Implementation** refers to a class that implements some interface, which we call the **Service Type**. A service implementation can depend on other services by requiring their type through the constructor.

```csharp
// Service Type
interface IFoo
{
    void FooMethod();
}

// Another service type
interface IBar
{
    int BarMethod();
}

// Service Implementation for type IFoo
class Foo : IFoo
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
    .Build();
```
When registering a service, we specify the **Service Type**, **Service Implementation**, and **Creation Strategy**.

In the example above, we registered the following services:

| Service Type | Service Implementation | Creation Strategy          |
|--------------|------------------------|----------------------------|
| `IFoo`       | `Foo`                  | Singleton                  |
| `IBar`       | `Bar`                  | Transient                  |
| `IBaz`       | `Baz`                  | Scoped                     |

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

## Service Factory Methods
**Service Factory Methods** are methods that create a new instance of a service. They are called whenever we request a service and the creation strategy creates a new instance. The default service factory method creates a new instance of the service and resolves its dependencies. 

We can define custom factory methods, which can be useful when we need to perform some custom logic when creating a service instance. Custom factory methods are defined when registering a service, and they are defined as delegates that take an `IServiceProvider` as an argument and return an instance of the service.

```csharp
IServiceProvider serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>() // Default factory method
    .RegisterTransient<IBar, Bar>(serviceProvider => {
        // Custom factory method
        return new Bar();
    }) 
    .RegisterScoped<IBaz, Baz>()
    .Build();
```

If a service depends on another service and we are using a custom factory method, we have to resolve the dependencies ourselves within the factory method.
```csharp
IServiceProvider serviceProvider = new ServiceProviderBuilder()
    .RegisterSingleton<IFoo, Foo>() // Default factory method
    .RegisterTransient<IBar, Bar>(serviceProvider => {
        // Custom factory method
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo); // Resolve the dependency (Bar depends on IFoo)
    }) 
    .RegisterScoped<IBaz, Baz>()
    .Build();
```

## Custom Creation Strategies
Custom creation strategies can be useful when we need to perform some custom logic for handling the lifetime of service instances. We can define custom creation strategies by implementing the `ICreationStrategy` interface. As an example, we will implement the `Transient` and `Singleton` creation strategies.

### Transient Creation Strategy Implementation
```csharp
class CustomTransientCreation: ICreationStrategy
{
    private readonly ServiceFactory serviceFactory;

    public TransientCreation(ServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    IServiceProvider serviceProvider
    public object GetInstance(IServiceProvider serviceProvider)
    {
        // Custom logic to create a service instance
        return this.serviceFactory(serviceProvider);
    }
}
```
It is good practice to decouple the instantitation logic from the creation strategy by requiring a `ServiceFactory` delegate in the constructor, which allows for custom service factory methods.

Now we can register services using our custom creation strategy. For the examples below, we will assume that `Bar` depends on `IFoo`.
```csharp
IServiceProvider serviceProvider = new ServiceProviderBuilder()
    .RegisterService<IFoo, Foo>(new CustomTransientCreation(
        DefaultServiceFactory.GetServiceFactory<Foo>())) // Default factory method
    .RegisterService<IBar, Bar>(new CustomTransientCreation((serviceProvider) => {
        // Custom factory method
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    }))
    .Build();
```

### Singleton Creation Strategy Implementation
```csharp
class CustomSingletonCreation: ICreationStrategy
{
    private readonly ServiceFactory serviceFactory;
    private object instance;
    public SingletonCreation(ServiceFactory serviceFactory) 
    {
        this.serviceFactory = serviceFactory;
    }

    public object GetInstance(IServiceProvider serviceProvider)
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
IServiceProvider serviceProvider = new ServiceProviderBuilder()
    .RegisterService<IFoo, Foo>(new CustomSingletonCreation(
        DefaultServiceFactory.GetServiceFactory<Foo>())) // Default factory method
    .RegisterService<IBar, Bar>(new CustomSingletonCreation((serviceProvider) => {
        // Custom factory method
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    }))
    .Build();
```

### Extending the Builder
Notice that currently, registering services with our custom creation strategies is a bit verbose, unlike the built-in creation strategies. We can extend the `ServiceProviderBuilder` to provide a more fluent API for registering services with custom creation strategies.

```csharp
public static class CustomServiceProvideBuilderExtensions
{
    public static IServiceProviderBuilder RegisterCustomTransient<TService, TImplementation>(
        this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
    {
        return builder.RegisterService<TService, TImplementation>(new CustomTransientCreation(serviceFactory));
    }

    public static IServiceProviderBuilder RegisterCustomSingleton<TService, TImplementation>(
        this IServiceProviderBuilder builder, ServiceFactory serviceFactory)
    {
        return builder.RegisterService<TService, TImplementation>(new CustomSingletonCreation(serviceFactory));
    }

    // Extension methods for using the default service factory method
    public static IServiceProviderBuilder RegisterCustomTransient<TService, TImplementation>(
        this IServiceProviderBuilder builder)
    {
        return builder.RegisterService<TService, TImplementation>(new CustomTransientCreation(
            DefaultServiceFactory.GetServiceFactory<TImplementation>()));
        ));
    }

    public static IServiceProviderBuilder RegisterCustomSingleton<TService, TImplementation>(
        this IServiceProviderBuilder builder)
    {
        return builder.RegisterService<TService, TImplementation>(new CustomSingletonCreation(
            DefaultServiceFactory.GetServiceFactory<TImplementation>()));
    }
}
```
Now the service registration process is much more fluent and streamlined.
```csharp
IServiceProvider serviceProvider = new ServiceProviderBuilder()
    .RegisterCustomSingleton<IFoo, Foo>() // Default factory method
    .RegisterCustomTransient<IBar, Bar>((serviceProvider) => {
        // Custom factory method
        IFoo foo = serviceProvider.GetService<IFoo>();
        return new Bar(foo);
    })
    .Build();
```