namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IWind
{
    /// <summary>
    /// 风向
    /// </summary>
    string WindDirection{ get; set; }
    /// <summary>
    /// 风力等级
    /// </summary>
    string WindScale { get; set; }
    /// <summary>
    /// 风速(km/h)
    /// </summary>
    int WindSpeed { get; set; }
}