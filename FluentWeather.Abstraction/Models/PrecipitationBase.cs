using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.Abstraction.Models
{
    
    /// <summary>
    /// 降水概率
    /// </summary>
    public class PrecipitationBase
    {
        public List<PrecipitationItemBase> Precipitations { get; set; }
    }
    public class PrecipitationItemBase
    {
        /// <summary>
        /// 降水量，单位毫米
        /// </summary>
        public double Precipitation { get; set; }
        public DateTime Time { get; set; }  
        public bool IsSnow { get; set; }
    }
}
