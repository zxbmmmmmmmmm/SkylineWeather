using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.OpenMeteoProvider.Models;

public class OpenMeteoAirCondition : AirConditionBase, IAirPollutants
{
    public double PM25 { get; set; }
    public double PM10 { get; set; }
    public double NO2 { get; set; }
    public double SO2 { get; set; }
    public double CO { get; set; }
    public double O3 { get; set; }
}