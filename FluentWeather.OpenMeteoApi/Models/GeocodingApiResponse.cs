using System.Text.Json.Serialization;

namespace FluentWeather.OpenMeteoApi.Models;

/// <summary>
/// Geocoding API response
/// </summary>
public class GeocodingApiResponse
{
    /// <summary>
    /// Array of found locations
    /// </summary>
    [JsonPropertyName("results")]
    public LocationData[]? Locations { get; set; }

    /// <summary>
    /// Generation time of the response in milliseconds.
    /// </summary>
    [JsonPropertyName("generationtime_ms")]
    public float GenerationTime { get; set; }
}