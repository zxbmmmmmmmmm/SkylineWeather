namespace FluentWeather.OpenMeteoApi.Models;

/// <summary>
/// Api response containing information about current weather conditions
/// </summary>
public class Current
{
    public string? Time { get; set; }

    public int? Interval { get; set; }

    /// <summary>
    /// Temperature in <see cref="WeatherForecastOptions.Temperature_Unit"/>
    /// </summary>
    public float? Temperature { get { return Temperature_2m; } private set { } } 

    public float? Temperature_2m { get; set; }

    /// <summary>
    /// WMO Weather interpretation code.
    /// To get an actual string representation use <see cref="OpenMeteo.OpenMeteoClient.WeathercodeToString(int)"/>
    /// </summary>
    public int? Weathercode { get; set; }

    /// <summary>
    /// Windspeed. Unit defined in <see cref="WeatherForecastOptions.Windspeed_Unit"/>
    /// </summary>
    /// <value></value>
    public float? Windspeed_10m { get; set; }

    /// <summary>
    /// Wind direction in degrees
    /// </summary>
    public int? Winddirection_10m { get; set; }
    public float? Windgusts_10m { get; set; }


    public int? Relativehumidity_2m { get; set; }
    public float? Apparent_temperature { get; set; }
    public int? Is_day { get; set; }
    public float? Precipitation { get; set; }
    public float? Rain { get; set; }
    public float? Showers { get; set; }
    public float? Snowfall { get; set; }
    public int? Cloudcover { get; set; }
    public float? Pressure_msl { get; set; }
    public float? Surface_pressure { get; set; }
}