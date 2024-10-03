namespace SkylineWeather.Abstractions.Models.Weather;

public class HalfDayWeather : Weather
{
    public bool IsDay { get; set; }
    public Wind? Wind { get; set; }
}