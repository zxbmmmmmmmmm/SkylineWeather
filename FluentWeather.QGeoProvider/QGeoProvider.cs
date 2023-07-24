using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.QGeoProvider
{
    public class QGeoProvider : IGeolocationProvider
    {
        public Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}