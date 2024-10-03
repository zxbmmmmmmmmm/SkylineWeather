using System.Diagnostics.CodeAnalysis;

namespace SkylineWeather.Abstractions.Models;

public enum WindDirection
{
    /// <summary>
    /// 北
    /// </summary>
    North,
    /// <summary>
    /// 南
    /// </summary>
    South,
    /// <summary>
    /// 东
    /// </summary>
    East,
    /// <summary>
    /// 西
    /// </summary>
    West,
    /// <summary>
    /// 西北
    /// </summary>
    NorthWest,
    /// <summary>
    /// 东北
    /// </summary>
    NorthEast,
    /// <summary>
    /// 西南
    /// </summary>
    SouthWest,
    /// <summary>
    /// 西北
    /// </summary>
    SouthEast,
    /// <summary>
    /// 东北偏北
    /// </summary>
    NorthNorthEast,
    /// <summary>
    /// 东北偏东	
    /// </summary>
    EastNorthEast,
    /// <summary>
    /// 东南偏东	
    /// </summary>
    EastSouthEast,
    /// <summary>
    /// 东南偏南	
    /// </summary>
    SouthSouthEast,
    /// <summary>
    /// 西南偏南	
    /// </summary>
    SouthSouthWest,
    /// <summary>
    /// 西南偏西	
    /// </summary>
    WestSouthWest,
    /// <summary>
    /// 西北偏西	
    /// </summary>
    WestNorthWest,
    /// <summary>
    /// 西北偏北	
    /// </summary>
    NorthNorthWest,
    /// <summary>
    /// 旋转风向
    /// </summary>
    Rotational,
    /// <summary>
    /// 无持续风向
    /// </summary>
    None,
}
public static class WindDirectionExtensions
{
    public static WindDirection GetWindDirectionFromAngle(double angle)
    {        
        if ((348.75 <= angle && angle <= 360) || (0 <= angle && angle <= 11.25)) return WindDirection.North;
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
}