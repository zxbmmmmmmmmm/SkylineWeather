using FluentWeather.Abstraction.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers
{
    public static class PrecipitationMapper
    {
        extension(PrecipitationResponse.PrecipitationItem item)
        {
            public PrecipitationItemBase MapToPrecipitationItemBase()
            {
                return new PrecipitationItemBase(item.FxTime, item.Precip, item.Type is "snow");
            }
        }

        extension(PrecipitationResponse item)
        {
            public PrecipitationBase MapToPrecipitationBase()
            {
                return new PrecipitationBase
                {
                    Summary = item.Summary,
                    Precipitations = item.MinutelyPrecipitations?.ConvertAll(p => p.MapToPrecipitationItemBase()),
                };
            }
        }
    }
}
