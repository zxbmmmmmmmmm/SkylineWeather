using UnitsNet;

namespace SkylineWeather.Abstractions.Models.AirQuality;

public record AirQuality
{
    public Density? PM25 { get; set; }

    public Density? PM10 { get; set; }

    public Density? SO2 { get; set; }

    public Density? NO2 { get; set; }

    public Density? O3 { get; set; }

    public Density? CO { get; set; }

    public Aqi? Aqi { get; set; }
}