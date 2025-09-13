using UnitsNet;
using UnitsNet.Units;

namespace SkylineWeather.Abstractions.Models.Weather;

public record HourlyWeather : Weather
{
    public DateTimeOffset Time { get; set; }

    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature Temperature { get; set; }

    public Wind? Wind { get; set; }

    public double? CloudAmount { get; set; }

    [DefaultUnit(LengthUnit.Kilometer)]
    public Length? Visibility { get; set; }

    public double? Humidity { get; set; }

    [DefaultUnit(PressureUnit.Hectopascal)]
    public Pressure? Pressure { get; set; }

}