using FluentWeather.Abstraction.Models;

using static FluentWeather.Abstraction.Models.WeatherCode;
namespace FluentWeather.QWeatherProvider.Helpers;


public static class WeatherTypeConverter
{
    public static WeatherCode GetWeatherTypeByDescription(string description)
    {
        if (description.Contains("晴"))
            return Clear;
        if (description.Contains("阴"))
            return Overcast;
        if (description.Contains("小雨"))       
            return SlightRain;      
        if (description.Contains("中雨"))       
            return ModerateRain;        
        if (description.Contains("大雨"))
            return HeavyRain;
        if (description.Contains("局部多云"))
            return PartlyCloudy;
        if (description.Contains("大部多云"))
            return PartlyCloudy;
        if (description.Contains("少云"))
            return MainlyClear;

        if (description.Contains("冰雹"))
            return SlightHail;
        if (description.Contains("雷阵雨"))
            return HeavyThunderStorm;
        if(description.Contains("小雪"))
            return SlightSnowFall;
        if (description.Contains("大雪"))
            return HeavySnowFall;


        if (description.Contains("雪"))
            return ModerateSnowFall;
        if (description.Contains("雷"))
            return SlightOrModerateThunderstorm;
        if (description.Contains("雨"))
            return ModerateRain;
        if (description.Contains("雾"))
            return Fog;
        if (description.Contains("霾"))
            return Haze;
        if (description.Contains("多云"))
            return PartlyCloudy;



        return Unknown;
    }
}