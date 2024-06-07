using System;

namespace FluentWeather.Abstraction.Models;

public class HistoricalDailyWeatherBase
{
    public DateTime Date { get; set; }

    public WeatherCode Weather { get; set; }

    public int HistoricalMaxTemperature { get; set; }

    public DateTime HistoricalMaxTemperatureDate { get; set; }

    public int HistoricalMinTemperature { get; set; }

    public DateTime HistoricalMinTemperatureDate { get; set; }

    public int AverageMaxTemperature { get; set; }

    public int AverageMinTemperature { get; set; }

    public double? AveragePrecipitation { get; set; }

    public double? MaxPrecipitation { get; set; }

    public DateTime? MaxPrecipitationDate { get; set; }
    public double? AveragePrecipitationHours { get; set; }
}