using UnitsNet;

namespace SkylineWeather.Abstractions.Models.Weather;

public class Wind
{
    public Speed? Speed { get; set; }
    public Angle? Angle { get; set; }
    public WindDirection Direction { get; set; }
}