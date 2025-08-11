namespace SkylineWeather.Abstractions.Provider;

/// <summary>
/// ָ���ṩ����������������͡�
/// </summary>
/// <param name="configurationType">���ö�������͡�</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ProviderConfigurationAttribute(Type configurationType) : Attribute
{
    /// <summary>
    /// ��ȡ���ö�������͡�
    /// </summary>
    public Type ConfigurationType { get; } = configurationType;
}