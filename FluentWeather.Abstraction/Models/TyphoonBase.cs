using FluentWeather.Abstraction.Interfaces.Weather;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FluentWeather.Abstraction.Models;

public class TyphoonBase
{
    public string Name { get; set; }
    public bool IsActive { get;set; }
    public List<TyphoonTrackBase> History { get; set; }
    public TyphoonTrackBase Now { get; set; }
    public List<TyphoonTrackBase> Forecast { get; set; }
}
public class TyphoonTrackBase:IPressure,ITime
{
    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; set; }
    /// <summary>
    /// 纬度
    /// </summary>
    public double Latitude { get; set; }
    public TyphoonType Type { get; set; }
    public int Pressure { get ; set; }
    public int WindSpeed { get; set; }
    public int? MoveSpeed { get; set; }
    public DateTime Time { get; set; }
}

/// <summary>
/// 根据GBT 19201-2006的台风类型
/// </summary>
public enum TyphoonType
{
    [Description("热带气压")]
    TD,
    [Description("热带风暴")]
    TS,
    [Description("强热带风暴")]
    STS,
    [Description("台风")]
    TY,
    [Description("强台风")]
    STY,
    [Description("超强台风")]
    SuperTY
}

