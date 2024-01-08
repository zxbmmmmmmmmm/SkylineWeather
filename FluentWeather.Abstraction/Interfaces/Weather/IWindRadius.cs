  using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.Abstraction.Interfaces.Weather;
#nullable enable
public interface IWindRadius
{
    WindRadius? WindRadius7 { get; set; }
    WindRadius? WindRadius10 { get; set; }
    WindRadius? WindRadius12 { get; set; }
}
public class WindRadius
{
    public int NorthEast { get; set; }
    public int NorthWest { get; set; }
    public int SouthEast { get; set; }
    public int SouthWest { get; set; }
}

