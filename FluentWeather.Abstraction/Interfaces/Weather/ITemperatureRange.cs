namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface ITemperatureRange
{
    int MaxTemperature { get; set; }
    int MinTemperature { get; set; }
}