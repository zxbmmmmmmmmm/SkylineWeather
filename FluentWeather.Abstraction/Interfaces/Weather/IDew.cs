namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IDew
{
    /// <summary>
    /// 露点温度
    /// </summary>
    public int? DewPointTemperature { get; set; }
}