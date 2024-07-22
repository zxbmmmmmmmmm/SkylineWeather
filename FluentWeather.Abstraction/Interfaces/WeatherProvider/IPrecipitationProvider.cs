using FluentWeather.Abstraction.Models;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface IPrecipitationProvider
{
    Task<PrecipitationBase> GetPrecipitations(double lon, double lat);
}

