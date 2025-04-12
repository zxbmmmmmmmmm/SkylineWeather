using UnitsNet;

namespace SkylineWeather.Abstractions.Models.Weather;

public record DailyWeather : Weather
{
    public DateOnly Date { get; set; }
    public Temperature HighTemperature { get; set; }
    public Temperature LowTemperature { get; set; }
    public TimeOnly? Sunrise { get; set; }
    public TimeOnly? Sunset { get; set; }
    public Wind? Wind { get; set; }
    public double? CloudAmount { get; set; }
    public Length? Visibility { get; set; }
    public double? Humidity { get; set; }
    public HalfDayWeather? DaytimeWeather { get; set; }
    public HalfDayWeather? NightWeather { get; set; }
}