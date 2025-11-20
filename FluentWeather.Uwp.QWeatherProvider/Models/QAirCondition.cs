using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.QWeatherProvider.Models
{
    public sealed class QAirCondition : AirConditionBase, IAirPollutants
    {
        public string Link { get; set; }
    }
}
