using UnitsNet;
using UnitsNet.Units;

namespace SkylineWeather.Abstractions.Models.Weather;

public record HistoricalWeather : Weather
{
    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature HighestTemperature { get; set; }

    public DateOnly HighestTemperatureDate { get; set; }

    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature LowestTemperature { get; set; }

    public DateOnly LowestTemperatureDate { get; set; }

    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature AverageHighTemperature { get; set; }

    [DefaultUnit(TemperatureUnit.DegreeCelsius)]
    public Temperature AverageLowTemperature { get; set; }
}