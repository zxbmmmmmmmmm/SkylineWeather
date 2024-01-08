using FluentWeather.Abstraction.Helpers;
using FluentWeather.OpenMeteoProvider.Helpers;
using FluentWeather.OpenMeteoProvider.Models;
using OpenMeteoApi.Models;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class AirConditionMapper
{
    public static OpenMeteoAirCondition MapToOpenMeteoWeatherNow(this AirQualityItem item)
    {
        return new OpenMeteoAirCondition
        {
            CO = (double)item.CarbonMonoxide!,
            NO2 = (double)item.NitrogenDioxide!,
            PM10 = (double)item.Pm10!,
            PM25 = (double)item.Pm25!,
            Aqi = (int)item.UsAqi!,//使用UsAqi
            SO2 = (double)item.SulphurDioxide!,
            O3 = (double)item.Ozone!,            
        };
    }

}