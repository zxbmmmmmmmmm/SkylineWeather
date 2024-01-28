namespace FluentWeather.Abstraction.Models;

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