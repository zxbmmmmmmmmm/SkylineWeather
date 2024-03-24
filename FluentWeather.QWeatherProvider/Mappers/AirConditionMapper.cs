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
                Aqi = item.Aqi,
                AqiCategory = item.Category,
                AqiLevel = item.Level,
                CO= item.Co,
                SO2 = item.So2,
                NO2 = item.No2,
                O3 = item.O3,
                PM10 = item.Pm10,
                PM25 = item.Pm2p5,
            };
        }
    }
}
