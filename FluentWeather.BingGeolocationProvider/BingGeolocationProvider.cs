using BingMapsRESTToolkit;
using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Models.Exceptions;
using FluentWeather.BingGeolocationProvider.Mappers;
using FluentWeather.Uwp.Shared;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Location = BingMapsRESTToolkit.Location;

namespace FluentWeather.BingGeolocationProvider;

public class BingGeolocationProvider : ProviderBase, IGeolocationProvider
{
    public override string Name => "BingMapsGeolocation";

    public override string Id => "bingmaps";

    public async Task<List<GeolocationBase>> GetCitiesGeolocationByLocation(double lat, double lon)
    {
        var request = new ReverseGeocodeRequest()
        {
            Culture = CultureInfo.CurrentCulture.ToString(),
            Point = new Coordinate(lat, lon),
            IncludeIso2 = true,
            IncludeNeighborhood = true,
            BingMapsKey = Constants.BingMapsKey,
        };

        //Process the request by using the ServiceManager.
        var response = await request.Execute();

        if (response is { ResourceSets.Length: > 0 } &&
            response.ResourceSets[0].Resources is { Length: > 0 })
        {
            return response.ResourceSets[0].Resources.Cast<Location>().ToList().ConvertAll(p => p.MapToGeolocation());
        }
        throw new HttpResponseException(response.ErrorDetails[0], (HttpStatusCode)response.StatusCode);
    }

    public async Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name)
    {
        //Create a request.
        var request = new GeocodeRequest()
        {
            Culture = CultureInfo.CurrentCulture.ToString(),
            Query = name,
            IncludeIso2 = true,
            IncludeNeighborhood = true,
            MaxResults = 25,
            BingMapsKey = Constants.BingMapsKey,
        };

        var response = await request.Execute();

        if (response is { ResourceSets.Length: > 0 })
        {
            if (response.ResourceSets[0].Resources is { Length: > 0 })
            {
                return response.ResourceSets[0].Resources.Cast<Location>().ToList().ConvertAll(p => p.MapToGeolocation());
            }

            return new List<GeolocationBase>();
        }
        throw new HttpResponseException(response.ErrorDetails[0], (HttpStatusCode)response.StatusCode);
    }
}