namespace FluentWeather.OpenMeteoApi.Models;

public class CurrentUnits
{
    public string? Time { get; set; }
    public string? Interval { get; set; }
    public string? Temperature_2m { get; set; }
    public string? Temperature { get { return Temperature_2m; } private set { } }
    public string? Relativehumidity_2m { get; set; }
    public string? Apparent_temperature { get; set; }
    public string? Is_day { get; set; }
    public string? Precipitation { get; set; }
    public string? Rain { get; set; }
    public string? Showers { get; set; }
    public string? Snowfall { get; set; }
    public string? Weathercode { get; set; }
    public string? Cloudcover { get; set; }
    public string? Pressure_msl { get; set; }
    public string? Surface_pressure { get; set; }
    public string? Windspeed_10m { get; set; }
    public string? Winddirection_10m { get; set; }
    public string? Windgusts_10m { get; set; }
}