using UnitsNet;
using UnitsNet.Units;

namespace SkylineWeather.Abstractions.Models.Weather;

public record Wind
{
    [DefaultUnit(SpeedUnit.KilometerPerHour)]
    public Speed? Speed { get; set; }

    [DefaultUnit(AngleUnit.Degree)]
    public Angle? Angle { get; set; }
    public WindDirection Direction { get; set; }
}