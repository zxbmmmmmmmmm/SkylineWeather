using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.QWeatherProvider.Models
{
    public sealed class QTyphoonTrack : TyphoonTrackBase, IWindRadius
    {
        public WindRadius WindRadius7 { get ; set ; }
        public WindRadius WindRadius10 { get ; set ; }
        public WindRadius WindRadius12 { get ; set ; }
    }
}
