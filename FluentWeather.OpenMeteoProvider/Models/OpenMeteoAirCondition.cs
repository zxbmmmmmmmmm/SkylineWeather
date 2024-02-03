using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.OpenMeteoProvider.Models;

public sealed class OpenMeteoAirCondition : AirConditionBase
{
    public override string? AqiCategory => UnitConverter.GetAqiCategoryUS(Aqi);
}