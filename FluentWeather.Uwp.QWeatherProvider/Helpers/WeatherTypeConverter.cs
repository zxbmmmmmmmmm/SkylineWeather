using FluentWeather.Abstraction.Models;
using static FluentWeather.Abstraction.Models.WeatherCode;
namespace FluentWeather.Uwp.QWeatherProvider.Helpers;


public static class WeatherTypeConverter
{
    public static WeatherCode GetWeatherTypeByDescription(string description)
    {
        description = description.ToLower();
        if (description.Contains("晴") || description.Contains("Clear"))
            return Clear;
        if (description.Contains("阴") || description.Contains("Overcast"))
            return Overcast;
        if (description.Contains("小雨") || description.Contains("Light Rain"))       
            return SlightRain;      
        if (description.Contains("中雨") || description.Contains("Moderate Rain"))       
            return ModerateRain;        
        if (description.Contains("大雨") || description.Contains("Heavy Rain"))
            return HeavyRain;
        if (description.Contains("局部多云") || description.Contains("Partly Cloudy"))
            return PartlyCloudy;
        if (description.Contains("大部多云") || description.Contains("Cloudy"))
            return PartlyCloudy;
        if (description.Contains("少云") || description.Contains("Mainly Clear"))
            return MainlyClear;

        if (description.Contains("冰雹") || description.Contains("Hail"))
            return SlightHail;
        if (description.Contains("雷阵雨") || description.Contains("Thunder"))
            return HeavyThunderStorm;
        if(description.Contains("小雪") || description.Contains("Light Snow"))
            return SlightSnowFall;
        if (description.Contains("大雪") || description.Contains("Heavy Snow"))
            return HeavySnowFall;
        if (description.Contains("冻雨") || description.Contains("Freezing Rain"))
            return LightFreezingRain;

        if (description.Contains("雪") || description.Contains("Snow"))
            return ModerateSnowFall;
        if (description.Contains("雷") || description.Contains("Thunder"))
            return SlightOrModerateThunderstorm;
        if (description.Contains("雨") || description.Contains("Rain"))
            return ModerateRain;
        if (description.Contains("雾") || description.Contains("Fog"))
            return Fog;
        if (description.Contains("霾") || description.Contains("Haze"))
            return Haze;
        if (description.Contains("多云") || description.Contains("Cloud"))
            return PartlyCloudy;



        return Unknown;
    }
}