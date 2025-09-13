using UnitsNet;
using UnitsNet.Units;

namespace SkylineWeather.Abstractions.Models.Weather;

public record DailyWeather : Weather
{
    public DateOnly Date { get; set; }

    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature HighTemperature { get; set; }

    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature LowTemperature { get; set; }

    public TimeOnly? Sunrise { get; set; }

    public TimeOnly? Sunset { get; set; }

    public Wind? Wind { get; set; }

    public double? CloudAmount { get; set; }

    [DefaultUnit(LengthUnit.Kilometer)]
    public Length? Visibility { get; set; }

    public double? Humidity { get; set; }

    public HalfDayWeather? DaytimeWeather { get; set; }

    public HalfDayWeather? NightWeather { get; set; }
}