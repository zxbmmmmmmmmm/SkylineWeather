namespace SkylineWeather.Abstractions.Models.Weather;

public class Wind
{
    public double? Speed { get; set; }
    public int? Angle { get; set; }
    public WindDirection Direction { get; set; }
}