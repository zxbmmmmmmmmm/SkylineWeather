namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IHumidity
{
    /// <summary>
    /// 湿度(百分比数值)
    /// </summary>
    int? Humidity { get; set; }
}