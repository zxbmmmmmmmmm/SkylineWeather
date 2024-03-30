using Microsoft.Extensions.DependencyInjection;

namespace FluentWeather.DIContainer;

public class Locator
{
    public static ServiceCollection ServiceDescriptors { get; } = new();
    public static ServiceProvider ServiceProvider;
}
