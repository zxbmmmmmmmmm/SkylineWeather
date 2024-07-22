﻿using FluentWeather.Abstraction.Interfaces.Weather;
using System;
using System.Collections.Generic;

namespace FluentWeather.Abstraction.Models
{
    
    /// <summary>
    /// 降水概率
    /// </summary>
    public class PrecipitationBase:ISummary
    {
        public List<PrecipitationItemBase> Precipitations { get; set; }
        public string? Summary { get; set; }
    }
    public class PrecipitationItemBase
    {
        /// <summary>
        /// 降水量，单位毫米
        /// </summary>
        public double Precipitation { get; set; }
        public DateTime Time { get; set; }  
        public bool IsSnow { get; set; }
        public PrecipitationItemBase(DateTime time,double precipitation,bool isSnow=false)
        {
            Precipitation = precipitation;
            Time = time;
            IsSnow = isSnow;
        }
    }
}
