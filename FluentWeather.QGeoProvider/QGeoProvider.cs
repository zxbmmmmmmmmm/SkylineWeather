using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.QGeoApi;
using FluentWeather.QGeoApi.ApiContracts;
using FluentWeather.QGeoProvider.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluentWeather.QGeoProvider
{
    public class QGeoProvider : IGeolocationProvider
    {
        public ApiHandlerOption Option { get; set; } = new();

        public async Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name)
        {
            var result = await RequestAsync(new GeolocationApi<QGeolocationResponse>(), new QGeolocationRequestByName{Name = name});
            return result.Locations.ConvertAll(p => (GeolocationBase)p.MapToQGeolocation());
        }
        public async Task<List<GeolocationBase>> GetCitiesGeolocationByLocation(double lat,double lon)
        {
            var result = await RequestAsync(new GeolocationApi<QGeolocationResponse>(), new QGeolocationRequestByLocation {Lat = lat,Lon = lon });
            return result.Locations.ConvertAll(p => (GeolocationBase)p.MapToQGeolocation());
        }
        public async Task<TResponse> RequestAsync<TResponse>(
            GeolocationApi<TResponse> contract, IQGeolocationRequest request)
        {
            var handler = new QGeoApiHandler();
            return await handler.RequestAsync(contract, request, Option);
        }
    }
}