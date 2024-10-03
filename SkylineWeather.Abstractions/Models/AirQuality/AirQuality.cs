namespace SkylineWeather.Abstractions.Models.AirQuality;

public class AirQuality
{
    public double? PM25 { get; set; }
    public double? PM10 { get; set; }
    public double? SO2 { get; set; }
    public double? NO2 { get; set; }
    public double? O3 { get; set; }
    public double? CO { get; set; }
    public IAqi? Aqi { get; set; }
}