using UnitsNet;

namespace SkylineWeather.Abstractions.Models.Weather;

public record HistoricalWeather : Weather
{
    public Temperature HighestTemperature { get; set; }

    public DateOnly HighestTemperatureDate { get; set; }

    public Temperature LowestTemperature { get; set; }

    public DateOnly LowestTemperatureDate { get; set; }

    public Temperature AverageHighTemperature { get; set; }

    public Temperature AverageLowTemperature { get; set; }
}