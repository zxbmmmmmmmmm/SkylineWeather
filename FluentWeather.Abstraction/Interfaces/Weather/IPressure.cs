namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IPressure
{
    /// <summary>
    /// 大气压强(百帕)
    /// </summary>
    public int? Pressure { get; set; }
}