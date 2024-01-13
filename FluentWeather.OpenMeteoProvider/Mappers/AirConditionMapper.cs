using FluentWeather.Abstraction.Helpers;
using FluentWeather.OpenMeteoProvider.Helpers;
using FluentWeather.OpenMeteoProvider.Models;
using OpenMeteoApi.Models;
using System;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class AirConditionMapper
{
    public static OpenMeteoAirCondition MapToOpenMeteoWeatherNow(this AirQualityItem item)
    {
        return new OpenMeteoAirCondition
        {
            CO = Math.Round((double)item.CarbonMonoxide!,2),
            NO2 = Math.Round((double)item.CarbonMonoxide!,2),
            PM10 = Math.Round((double)item.Pm10!, 2),
            PM25 = Math.Round((double)item.Pm25!, 2),
            Aqi = (int)item.UsAqi!,//使用UsAqi
            SO2 = Math.Round((double)item.SulphurDioxide!, 2),
            O3 = Math.Round((double)item.Ozone!, 2),            
        };
    }

}