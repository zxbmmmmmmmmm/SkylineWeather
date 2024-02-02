using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.QWeatherProvider.Models
{
    public sealed class QTyphoon:TyphoonBase
    {
    }
    public sealed class QTyphoonTrack : TyphoonTrackBase, IWindRadius
    {
        public WindRadius WindRadius7 { get ; set ; }
        public WindRadius WindRadius10 { get ; set ; }
        public WindRadius WindRadius12 { get ; set ; }
    }
}
