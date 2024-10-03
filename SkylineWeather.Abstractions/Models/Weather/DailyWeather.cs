namespace SkylineWeather.Abstractions.Models.Weather;

public class DailyWeather : Weather
{
    public DateOnly Date { get; set; }
    public double HighTemperature { get; set; }
    public double LowTemperature { get; set; }
    public TimeOnly? Sunrise { get; set; }
    public TimeOnly? Sunset { get; set; }
    public Wind? Wind { get; set; }
    public double? CloudAmount { get; set; }
    public double? Visibility { get; set; }
    public double? Humidity { get; set; }
    public HalfDayWeather? DaytimeWeather { get; set; }
    public HalfDayWeather? NightWeather { get; set; }
}