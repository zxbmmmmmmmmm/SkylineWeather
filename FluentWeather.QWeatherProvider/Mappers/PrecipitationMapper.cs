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
        public static QWeatherPrecipitationItem MapToQweatherPrecipitationItem(this PrecipitationResponse.PrecipitationItem item)
        {
            return new QWeatherPrecipitationItem
            {
                 Precipitation = double.Parse(item.Precip),
                 Time = DateTime.Parse(item.FxTime),
                 IsSnow = item.Type is "snow"
            };
        }
        public static QWeatherPrecipitation MapToQweatherPrecipitation(this PrecipitationResponse item) 
        {
            return new QWeatherPrecipitation
            {
                Summary = item.Summary,
                Precipitations = item.MinutelyPrecipitations?.ConvertAll(p => (PrecipitationItemBase)p.MapToQweatherPrecipitationItem()),
            };
        }
    }
}
