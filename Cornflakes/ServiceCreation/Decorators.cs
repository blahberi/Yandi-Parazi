using Cornflakes.Extensions;

namespace Cornflakes.ServiceCreation;

public delegate object DecoratorFactory(IServiceProvider serviceProvider, object instance);