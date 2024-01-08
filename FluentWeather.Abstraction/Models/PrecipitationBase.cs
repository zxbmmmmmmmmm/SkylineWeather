using FluentWeather.Abstraction.Interfaces.Weather;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.Abstraction.Models
{
    
    /// <summary>
    /// 降水概率
    /// </summary>
    public class PrecipitationBase:ISummary
    {
        public required List<PrecipitationItemBase> Precipitations { get; set; }
        public string? Summary { get; set; }
    }
    public class PrecipitationItemBase
    {
        /// <summary>
        /// 降水量，单位毫米
        /// </summary>
        public required double Precipitation { get; set; }
        public required DateTime Time { get; set; }  
        public bool IsSnow { get; set; }
    }
}
