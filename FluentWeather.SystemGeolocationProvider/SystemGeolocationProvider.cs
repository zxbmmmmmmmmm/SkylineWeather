using System;
using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Services.Maps.LocalSearch;

namespace FluentWeather.SystemGeolocationProvider;

public class SystemGeolocationProvider : ProviderBase, IGeolocationProvider
{
    public async Task<List<GeolocationBase>> GetCitiesGeolocationByLocation(double lat, double lon)
    {
        var location = new BasicGeoposition
        {
            Latitude = lat,
            Longitude = lon
        };
        var pointToReverseGeocode = new Geopoint(location);

        // Reverse geocode the specified geographic location.
        var result =
            await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
        throw new System.NotImplementedException();
    }

    public async Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name)
    {
        var addressToGeocode = "Microsoft";

        // The nearby location to use as a query hint.
        BasicGeoposition queryHint = new BasicGeoposition();
        queryHint.Latitude = 47.643;
        queryHint.Longitude = -122.131;
        Geopoint hintPoint = new Geopoint(queryHint);

        // Geocode the specified address, using the specified reference point
        // as a query hint. Return no more than 3 results.
        MapLocationFinderResult result =
            await MapLocationFinder.FindLocationsAsync(addressToGeocode);
    }

    public override string Name => "SystemGeolocation";
    public override string Id => "system_geolocation";
}