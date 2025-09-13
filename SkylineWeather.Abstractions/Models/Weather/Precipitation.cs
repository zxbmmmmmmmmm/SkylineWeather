using UnitsNet;
using UnitsNet.Units;

namespace SkylineWeather.Abstractions.Models.Weather;

public record Precipitation
{
    public DateTimeOffset Time { get; set; }

    [DefaultUnit(LengthUnit.Millimeter)]
    public Length Amount { get; set; }

    public PrecipitationType Type { get; set; }
}