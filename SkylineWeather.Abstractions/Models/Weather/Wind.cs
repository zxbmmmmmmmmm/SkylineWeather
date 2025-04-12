using UnitsNet;

namespace SkylineWeather.Abstractions.Models.Weather;

public record Wind
{
    public Speed? Speed { get; set; }
    public Angle? Angle { get; set; }
    public WindDirection Direction { get; set; }
}