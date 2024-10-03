namespace SkylineWeather.Abstractions.Models.Weather;

public class HourlyWeather : Weather
{
    public DateTimeOffset Time { get; set; }
    public double Temperature { get; set; }
    public Wind? Wind { get; set; }
    public double? CloudAmount { get; set; }
    public double? Visibility { get; set; }
    public double? Humidity { get; set; }
    public double? Pressure { get; set; }

}