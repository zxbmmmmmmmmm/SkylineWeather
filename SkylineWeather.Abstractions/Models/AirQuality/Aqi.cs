using UnitsNet;

namespace SkylineWeather.Abstractions.Models.AirQuality;

public record Aqi
{
    public required double Value { get; set; }

    public required AqiStandard Standard { get; set; }

    public AqiLevelDescriptor? LevelDescriptor { get; set; }

    public SubAqi SubAqis { get; set; } = new();
}

public record SubAqi
{
    public double? PM25 { get; set; }

    public double? PM10 { get; set; }

    public double? SO2 { get; set; }

    public double? NO2 { get; set; }

    public double? O3 { get; set; }

    public double? CO { get; set; }
}