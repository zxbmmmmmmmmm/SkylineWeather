using UnitsNet;
namespace SkylineWeather.Abstractions.Models.Weather;

public class CurrentWeather : Weather
{
    public Temperature Temperature { get; set; }
    public Wind? Wind { get; set; }
    public double? Visibility { get; set; }
    public double? CloudAmount { get; set; }
    public double? Humidity { get; set; }
}