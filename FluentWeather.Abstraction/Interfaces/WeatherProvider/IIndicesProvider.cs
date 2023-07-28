using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface IIndicesProvider
{
    Task<List<IndicesBase>> GetIndices(double lon, double lat);

}