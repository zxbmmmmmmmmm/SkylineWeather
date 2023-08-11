namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IPrecipitationProbability
{
    /// <summary>
    /// 降水概率
    /// </summary>
    int? PrecipitationProbability{ get; set; }
}