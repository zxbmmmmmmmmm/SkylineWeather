using FluentWeather.Abstraction.Helpers;

namespace FluentWeather.Abstraction.Models;

public class WeatherBase
{
    public virtual WeatherCode WeatherType { get; set; }

    private string? _description;

    public string Description
    {
        get => _description ?? ResourcesHelper.GetWeatherDescription((int)WeatherType);
        set => _description = value;
    }
}

