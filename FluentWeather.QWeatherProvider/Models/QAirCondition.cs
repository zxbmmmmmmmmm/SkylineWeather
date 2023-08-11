using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.QWeatherProvider.Models
{
    public class QAirCondition:AirConditionBase,IAirPollutants
    {
        public string Link{ get; set; }
        public double PM25 { get; set; }
        public double PM10 { get; set; }
        public double NO2 { get; set; }
        public double SO2 { get; set; }
        public double CO { get; set; }
        public double O3 { get; set; }
    }
}
