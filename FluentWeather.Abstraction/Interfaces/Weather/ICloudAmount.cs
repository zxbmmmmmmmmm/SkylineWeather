namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface ICloudAmount
{
    /// <summary>
    /// 云量
    /// </summary>
    int? CloudAmount { get; set; }
}