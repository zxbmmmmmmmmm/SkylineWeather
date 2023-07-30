using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentWeather.QWeatherProvider.Mappers
{
    internal static class AirConditionMapper
    {
        public static QAirCondition MapToQAirCondition(this AirConditionResponse.AirConditionItem item)
        {
            return new QAirCondition
            {
                Aqi = int.Parse(item.Aqi),
                AqiCategory = item.Category,
                AqiLevel = int.Parse(item.Level)
            };
        }
    }
}
