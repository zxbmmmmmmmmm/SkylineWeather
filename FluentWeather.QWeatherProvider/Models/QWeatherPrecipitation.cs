using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.QWeatherProvider.Models
{
    public class QWeatherPrecipitation : PrecipitationBase, ISummary
    {
        public string Summary { get ; set ; }
    }

    public class QWeatherPrecipitationItem:PrecipitationItemBase
    {
    }
}
