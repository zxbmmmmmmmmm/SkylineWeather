using FluentWeather.Abstraction.Models;
using static FluentWeather.Abstraction.Models.WeatherCode;
namespace FluentWeather.Uwp.QWeatherProvider.Helpers;


public static class WeatherTypeConverter
{
    public static WeatherCode GetWeatherTypeByDescription(string description)
    {
        description = description.ToLower();
        if (description.Contains("晴") || description.Contains("clear"))
            return Clear;
        if (description.Contains("阴") || description.Contains("overcast"))
            return Overcast;
        if (description.Contains("小雨") || description.Contains("light rain"))       
            return SlightRain;      
        if (description.Contains("中雨") || description.Contains("moderate rain"))       
            return ModerateRain;        
        if (description.Contains("大雨") || description.Contains("heavy rain"))
            return HeavyRain;
        if (description.Contains("局部多云") || description.Contains("partly cloudy"))
            return PartlyCloudy;
        if (description.Contains("大部多云") || description.Contains("cloudy"))
            return PartlyCloudy;
        if (description.Contains("少云") || description.Contains("mainly clear"))
            return MainlyClear;

        if (description.Contains("冰雹") || description.Contains("hail"))
            return SlightHail;
        if (description.Contains("雷阵雨") || description.Contains("thunder"))
            return HeavyThunderStorm;
        if(description.Contains("小雪") || description.Contains("light snow"))
            return SlightSnowFall;
        if (description.Contains("大雪") || description.Contains("heavy snow"))
            return HeavySnowFall;
        if (description.Contains("冻雨") || description.Contains("freezing rain"))
            return LightFreezingRain;

        if (description.Contains("雪") || description.Contains("snow"))
            return ModerateSnowFall;
        if (description.Contains("雷") || description.Contains("thunder"))
            return SlightOrModerateThunderstorm;
        if (description.Contains("雨") || description.Contains("rain"))
            return ModerateRain;
        if (description.Contains("雾") || description.Contains("fog"))
            return Fog;
        if (description.Contains("霾") || description.Contains("haze"))
            return Haze;
        if (description.Contains("多云") || description.Contains("cloud"))
            return PartlyCloudy;



        return Unknown;
    }
}