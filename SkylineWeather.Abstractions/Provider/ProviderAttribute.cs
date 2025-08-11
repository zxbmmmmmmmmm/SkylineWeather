namespace SkylineWeather.Abstractions.Provider;

[AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
public class ProviderAttribute(string id) : Attribute
{
    public string Id { get; set; } = id;
}