using UnitsNet;

namespace SkylineWeather.Abstractions.Models.Weather;

public class HourlyWeather : Weather
{
    public DateTimeOffset Time { get; set; }
    public Temperature Temperature { get; set; }
    public Wind? Wind { get; set; }
    public double? CloudAmount { get; set; }
    public Length? Visibility { get; set; }
    public double? Humidity { get; set; }
    public Pressure? Pressure { get; set; }

}