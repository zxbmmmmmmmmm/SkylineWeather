namespace SkylineWeather.Abstractions.Provider;

/// <summary>
/// 指定提供程序所需的配置类型。
/// </summary>
/// <param name="configurationType">配置对象的类型。</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ProviderConfigurationAttribute(Type configurationType) : Attribute
{
    /// <summary>
    /// 获取配置对象的类型。
    /// </summary>
    public Type ConfigurationType { get; } = configurationType;
}