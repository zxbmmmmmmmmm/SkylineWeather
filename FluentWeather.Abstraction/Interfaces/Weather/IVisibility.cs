namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IVisibility
{
    /// <summary>
    /// 能见度(公里)
    /// </summary>
    int? Visibility{ get; set; }
}