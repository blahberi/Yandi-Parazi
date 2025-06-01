using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Cornflakes;

public static class DependencyResolver
{
    public static ServiceFactory GetServiceFactory<TImplementation>()
    {
        return GenerateFactory(typeof(TImplementation));
    }

    public static ServiceCreationPipeline UseMemberInjection<TImplementation>(this ServiceCreationPipeline serviceCreationPipeline)
    {
        ServiceCreationPipeline? injector = TryCreateMemberInjector(typeof(TImplementation));
        return injector == null ? serviceCreationPipeline : serviceCreationPipeline.Attach(injector);
    }

    public static ServiceCreationPipeline UseMemberInjection<TImplementation>(this ServiceFactory serviceFactory)
    {
        return serviceFactory.ToPipeline().UseMemberInjection<TImplementation>();
    }

    private static ServiceFactory GenerateFactory(Type implementationType)
    {
        ParameterExpression serviceProviderParameter = Expression.Parameter(typeof(IProviderOfServices), "serviceProvider");
        MethodInfo? GetService = typeof(IProviderOfServices)
            .GetMethod(nameof(IProviderOfServices.GetService));

        if (GetService == null)
        {
            throw new MissingMethodException(nameof(IProviderOfServices), nameof(IProviderOfServices.GetService));
        }

        ConstructorInfo constructor = implementationType.GetConstructors().First();
        IEnumerable<Expression> arguments = constructor.GetParameters()
            .Select(p => Expression.Convert(
                Expression.Call(
                    serviceProviderParameter,
                    GetService,
                    Expression.Constant(p.ParameterType)), 
                p.ParameterType));

        Expression constructionExpression = Expression.New(constructor, arguments);

        return Expression.Lambda<ServiceFactory>(constructionExpression, serviceProviderParameter).Compile();
    }
    
    private static ServiceCreationPipeline? TryCreateMemberInjector(Type implementationType)
    {
        FieldInfo[] fields = implementationType
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(f => f.IsDefined(typeof(InjectAttribute), inherit: true))
            .ToArray();

        PropertyInfo[] props = implementationType
            .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(p => p.IsDefined(typeof(InjectAttribute), inherit: true) && p.CanWrite)
            .ToArray();

        if (fields.Length == 0 && props.Length == 0)
            return null;

        DynamicMethod dm = new DynamicMethod(
            $"Inject_{implementationType.Name}",
            returnType: null,
            parameterTypes: new[]
            {
                typeof(IProviderOfServices),
                typeof(object).MakeByRefType()
            },
            m: implementationType.Module,
            skipVisibility: true);

        ILGenerator il = dm.GetILGenerator();

        LocalBuilder typedTarget = il.DeclareLocal(implementationType);

        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldind_Ref);
        il.Emit(OpCodes.Castclass, implementationType);
        il.Emit(OpCodes.Stloc, typedTarget);

        foreach (FieldInfo field in fields)
        {
            il.Emit(OpCodes.Ldloc, typedTarget);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldtoken, field.FieldType);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod(
                nameof(Type.GetTypeFromHandle))!);
            il.Emit(OpCodes.Callvirt, typeof(IProviderOfServices)
                .GetMethod(nameof(IProviderOfServices.GetService))!);
            il.Emit(OpCodes.Castclass, field.FieldType);
            il.Emit(OpCodes.Stfld, field);
        }

        foreach (PropertyInfo prop in props)
        {
            MethodInfo? set = prop.GetSetMethod(nonPublic: true);
            if (set is null) continue;

            il.Emit(OpCodes.Ldloc, typedTarget);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldtoken, prop.PropertyType);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod(
                nameof(Type.GetTypeFromHandle))!);
            il.Emit(OpCodes.Callvirt, typeof(IProviderOfServices)
                .GetMethod(nameof(IProviderOfServices.GetService))!);
            il.Emit(OpCodes.Castclass, prop.PropertyType);
            il.Emit(OpCodes.Callvirt, set);
        }

        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldloc, typedTarget);
        il.Emit(OpCodes.Stind_Ref);

        il.Emit(OpCodes.Ret);

        return dm.CreateDelegate<ServiceCreationPipeline>();
    }
}
