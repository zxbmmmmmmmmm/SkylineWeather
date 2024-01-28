using System.Text.Json.Serialization;

namespace FluentWeather.OpenMeteoApi.Models;

/// <summary>
/// Weather Forecast API response
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// WGS84 of the center of the weather grid-cell which was used to generate this forecast. 
    /// This coordinate might be up to 5 km away.
    /// </summary>
    public float Latitude { get; set; }

    /// <summary>
    /// WGS84 of the center of the weather grid-cell which was used to generate this forecast. 
    /// This coordinate might be up to 5 km away.
    /// </summary>
    public float Longitude { get; set; }

    /// <summary>
    /// The elevation in meters of the selected weather grid-cell.
    /// </summary>
    public float Elevation { get; set; }

    /// <summary>
    /// Generation time of the weather forecast in milliseconds.
    /// </summary>
    [JsonPropertyName("generationtime_ms")]
    public float GenerationTime { get; set; }

    /// <summary>
    /// Applied timezone offset from 
    /// the <see cref="WeatherForecastOptions.Timezone"/> parameter.
    /// </summary>
    /// <value></value>

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffset { get; set; }

    /// <summary>
    /// Timezone identifier
    /// </summary>
    /// <example>Europe/Berlin</example>
    public string? Timezone { get; set; }

    /// <summary>
    /// Timezone abbreviation
    /// </summary>
    /// <example>CEST</example>

    [JsonPropertyName("timezone_abbreviation")]
    public string? TimezoneAbbreviation { get; set; }

    /// <summary>
    /// Current weather conditions
    /// </summary>

    [JsonPropertyName("current")]
    public Current? Current { get; set; }

    [JsonPropertyName("current_units")]
    public CurrentUnits? CurrentUnits { get; set; }

    /// <summary>
    /// For each selected <see cref="HourlyOptionsParameter"/>, the unit will be listed here
    /// </summary>

    [JsonPropertyName("hourly_units")]
    public HourlyUnits? HourlyUnits { get; set; }

    /// <summary>
    /// For each selected weather variable, data will be returned as a floating point array. 
    /// Additionally a time array will be returned with ISO8601 timestamps.
    /// </summary>

    [JsonPropertyName("hourly")]
    public Hourly? Hourly { get; set; }

    /// <summary>
    /// For each selected <see cref="DailyOptionsParameter"/>, the unit will be listed here
    /// </summary>

    [JsonPropertyName("daily_units")]
    public DailyUnits? DailyUnits { get; set; }

    /// <summary>
    /// For each selected weather variable, data will be returned as a floating point array. 
    /// Additionally a time array will be returned with ISO8601 timestamps.
    /// </summary>

    [JsonPropertyName("daily")]
    public Daily? Daily { get; set; }

    [JsonPropertyName("minutely_15")]
    public Minutely15? Minutely15 { get; set; }

    [JsonPropertyName("minutely_15_units")]
    public Minutely15Units? Minutely15Units { get; set; }
}