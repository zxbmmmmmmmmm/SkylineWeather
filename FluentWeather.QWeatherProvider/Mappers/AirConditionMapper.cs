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
                AqiLevel = int.Parse(item.Level),
                CO= double.Parse(item.Co),
                SO2 = double.Parse(item.So2),
                NO2 = double.Parse(item.No2),
                O3 = double.Parse(item.O3),
                PM10 = double.Parse(item.Pm10),
                PM25 = double.Parse(item.Pm2p5),
            };
        }
    }
}
