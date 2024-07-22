using FluentWeather.Abstraction.Models;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider
{
    public interface IAirConditionProvider
    {
        Task<AirConditionBase> GetAirCondition(double lon, double lat);

    }
}
