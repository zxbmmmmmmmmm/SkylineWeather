namespace FluentWeather.OpenMeteoApi.Models;

public class Minutely15
{
    public string[]? time { get; set; }
    public float[]? temperature_2m { get; set; }
    public int[]? relativehumidity_2m { get; set; }
    public float[]? dewpoint_2m { get; set; }
    public float[]? apparent_temperature { get; set; }
    public float[]? precipitation { get; set; }
    public float[]? rain { get; set; }
    public float[]? snowfall { get; set; }
    public float?[]? snowfall_height { get; set; }
    public float[]? freezinglevel_height { get; set; }
    public int[]? weathercode { get; set; }
    public float[]? windspeed_10m { get; set; }
    public float[]? windspeed_80m { get; set; }
    public int[]? winddirection_10m { get; set; }
    public int[]? winddirection_80m { get; set; }
    public float[]? windgusts_10m { get; set; }
    public float[]? visibility { get; set; }
    public float[]? cape { get; set; }
    public float?[]? lightning_potential { get; set; }
    public float[]? shortwave_radiation { get; set; }
    public float[]? direct_radiation { get; set; }
    public float[]? diffuse_radiation { get; set; }
    public float[]? direct_normal_irradiance { get; set; }
    public float[]? terrestrial_radiation { get; set; }
    public float[]? shortwave_radiation_instant { get; set; }
    public float[]? direct_radiation_instant { get; set; }
    public float[]? diffuse_radiation_instant { get; set; }
    public float[]? direct_normal_irradiance_instant { get; set; }
    public float[]? terrestrial_radiation_instant { get; set; }
}