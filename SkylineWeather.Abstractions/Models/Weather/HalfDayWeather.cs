namespace SkylineWeather.Abstractions.Models.Weather;

public record HalfDayWeather : Weather
{
    public bool IsDay { get; set; }
    public Wind? Wind { get; set; }
}