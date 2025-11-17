using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Strings;

namespace FluentWeather.Abstraction.Helpers;

public static class UnitConverter
{
    /// <summary>
    /// 将风速(KM/H)转换为风力等级
    /// </summary>
    /// <param name="speed">风速(KM/H)</param>
    /// <returns></returns>
    public static int GetWindScaleFromKM(int speed)
    {
        if(speed <= 2)return 0;
        if (speed <= 6) return 1;
        if (speed <= 12) return 2;
        if (speed <= 19) return 3;
        if (speed <= 30) return 4;
        if (speed <= 40) return 5;
        if (speed <= 51) return 6;
        if (speed <= 62) return 7;
        if (speed <= 75) return 8;
        if (speed <= 87) return 9;
        if (speed <= 103) return 10;
        if (speed <= 117) return 11;
        if (speed <= 132) return 12;
        if (speed <= 149) return 13;
        if (speed <= 166) return 14;
        if (speed <= 184) return 15;
        if (speed <= 201) return 16;
        if (speed <= 219) return 17;
        return 18;//17以上
    }

    /// <summary>
    /// 将风向角转换为风向
    /// </summary>
    /// <param name="angle">角度</param>
    /// <returns></returns>
    public static WindDirection GetWindDirectionFromAngle(int angle)
    {
        if ((348.75 <= angle && angle <= 360)|| (0<=angle && angle <= 11.25)) return WindDirection.North;
        if (11.25 < angle && angle <= 33.75) return WindDirection.NorthNorthEast;
        if (33.75 < angle && angle <= 56.25) return WindDirection.NorthEast;
        if (56.25 < angle && angle <= 78.75) return WindDirection.EastNorthEast;
        if (78.75 < angle && angle <= 101.25) return WindDirection.East;
        if (101.25 < angle && angle <= 123.75) return WindDirection.EastSouthEast;
        if (123.75 < angle && angle <= 146.25) return WindDirection.SouthEast;
        if (146.25 < angle && angle <= 168.75) return WindDirection.SouthSouthEast;
        if (168.75 < angle && angle <= 191.25) return WindDirection.South;
        if (191.25 < angle && angle <= 213.75) return WindDirection.SouthSouthWest;
        if (213.75 < angle && angle <= 236.25) return WindDirection.SouthWest;
        if (236.25 < angle && angle <= 258.75) return WindDirection.WestSouthWest;
        if (258.75 < angle && angle <= 281.25) return WindDirection.West;
        if (281.25 < angle && angle <= 303.75) return WindDirection.WestNorthWest;
        if (303.75 < angle && angle <= 326.25) return WindDirection.NorthWest;
        if (326.25 < angle && angle <= 348.75) return WindDirection.NorthNorthWest;
        if (angle is -999) return WindDirection.Rotational;
        return WindDirection.None;
    }

    /// <summary>
    /// Aqi(US)转换
    /// </summary>
    /// <returns></returns>
    public static string GetAqiCategoryUS(int aqi)
    {
        if (0 <= aqi && aqi <= 50) return Resources.ResourceManager.GetString("USAQI_Good")!;
        if (51 <= aqi && aqi <= 100) return Resources.ResourceManager.GetString("USAQI_Moderate")!;
        if (101 <= aqi && aqi <= 150) return Resources.ResourceManager.GetString("USAQI_UnhealthyForSensitiveGroups")!;
        if (151 <= aqi && aqi <= 200) return Resources.ResourceManager.GetString("USAQI_Unhealthy")!;
        if (201 <= aqi && aqi <= 300) return Resources.ResourceManager.GetString("USAQI_VeryUnhealthy")!;
        return Resources.ResourceManager.GetString("USAQI_Hazardous")!;
    }

    extension(double celsiusTemperature)
    {
        public double ToFahrenheit()
        {
            return celsiusTemperature * 9 / 5 + 32;
        }
    }

    extension(int celsiusTemperature)
    {
        public int ToFahrenheit()
        {
            return celsiusTemperature * 9 / 5 + 32;
        }
    }
}