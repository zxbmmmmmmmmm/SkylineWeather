using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.QWeatherProvider.Mappers
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
