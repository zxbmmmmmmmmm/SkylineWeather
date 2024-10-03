namespace QWeatherProvider;

public class QWeatherProviderConfig(string key)
{
    public string Token { get; set; } = key;
    public string? Language { get; set; }
    public string Domain { get; set; } = "devapi.qweather.com";
}