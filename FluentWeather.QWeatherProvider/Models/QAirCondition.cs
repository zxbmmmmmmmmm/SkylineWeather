using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.QWeatherProvider.Models
{
    public class QAirCondition:AirConditionBase
    {
        public string Link{ get; set; }
    }
}
