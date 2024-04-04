using FluentWeather.Abstraction.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers
{
    public static class PrecipitationMapper
    {
        public static PrecipitationItemBase MapToPrecipitationItemBase(this PrecipitationResponse.PrecipitationItem item)
        {
            return new PrecipitationItemBase
            {
                 Precipitation = item.Precip,
                 Time = item.FxTime,
                 IsSnow = item.Type is "snow"
            };
        }
        public static PrecipitationBase MapToPrecipitationBase(this PrecipitationResponse item) 
        {
            return new PrecipitationBase
            {
                Summary = item.Summary,
                Precipitations = item.MinutelyPrecipitations?.ConvertAll(p => (PrecipitationItemBase)p.MapToPrecipitationItemBase()),
            };
        }
    }
}
