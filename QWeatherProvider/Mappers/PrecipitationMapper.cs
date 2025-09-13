using QWeatherApi.ApiContracts;
using QWeatherProvider.Helpers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using UnitsNet;

namespace QWeatherProvider.Mappers;

public static class PrecipitationMapper
{
    public static Precipitation MapToPrecipitation(this PrecipitationResponse.PrecipitationItem current)
    {
        return new Precipitation
        {
            Amount = Length.FromMillimeters(current.Precip),
            Time = current.FxTime,
            Type = current.Type switch
            {
                "rain" => PrecipitationType.Rain,
                "snow" => PrecipitationType.Snow,
                _ => PrecipitationType.Rain,
            }
        };
    }
}