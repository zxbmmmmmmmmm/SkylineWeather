using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface ITyphoonProvider
{
    Task<List<TyphoonBase>> GetActiveTyphoons();
}

