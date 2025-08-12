namespace QWeatherProvider;

public class QWeatherProviderConfig
{
    public required string Token { get; set; }
    public string? Language { get; set; }
    public string Domain { get; set; } = "devapi.qweather.com";
}