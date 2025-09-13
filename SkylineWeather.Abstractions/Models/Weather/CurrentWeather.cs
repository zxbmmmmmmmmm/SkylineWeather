using UnitsNet;
using UnitsNet.Units;
namespace SkylineWeather.Abstractions.Models.Weather;

public record CurrentWeather : Weather
{
    // required
    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature Temperature { get; set; }
    public DailyWeather? Today { get; set; }
    public Wind? Wind { get; set; }
    public double? Visibility { get; set; }
    public double? CloudAmount { get; set; }
    public double? Humidity { get; set; }
}