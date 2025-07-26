using UnitsNet;
namespace SkylineWeather.Abstractions.Models.Weather;

public record CurrentWeather : Weather
{
    public required Temperature Temperature { get; set; }
    public DailyWeather? Today { get; set; }
    public Wind? Wind { get; set; }
    public double? Visibility { get; set; }
    public double? CloudAmount { get; set; }
    public double? Humidity { get; set; }
}