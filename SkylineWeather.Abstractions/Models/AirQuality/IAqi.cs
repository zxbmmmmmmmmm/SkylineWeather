namespace SkylineWeather.Abstractions.Models.AirQuality;

public interface IAqi
{
    public double Aqi { get; set; }
    public double Level { get; set; }
}