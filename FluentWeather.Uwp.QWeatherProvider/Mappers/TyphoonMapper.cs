using System;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;
using static QWeatherApi.ApiContracts.TyphoonTrackItem;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;
public static class TyphoonMapper
{
    extension(TyphoonTrackResponse item)
    {
        public TyphoonBase MapToTyphoonBase(string name)
        {
            return new TyphoonBase
            {
                Name = name,
                IsActive = item.IsActive is "1",
                History = item.Tracks.ConvertAll(p => (TyphoonTrackBase)p.MapToQTyphoonTrack()),
                Now = item.Now.MapToQTyphoonTrack(),
            };
        }
    }

    extension(TyphoonTrackItem item)
    {
        public QTyphoonTrack MapToQTyphoonTrack()
        {
            return new QTyphoonTrack
            {
                Latitude = item.Lat,
                Longitude = item.Lon,
                MoveSpeed = item.MoveSpeed,
                WindSpeed = item.WindSpeed,
                Pressure = item.Pressure,
                WindRadius7 = item.WindRadius7?.MapToWindRadius(),
                WindRadius10 = item.WindRadius10?.MapToWindRadius(),
                WindRadius12 = item.WindRadius12?.MapToWindRadius(),
                Type = (TyphoonType)Enum.Parse(typeof(TyphoonType), item.Type),
                Time = item.Time ?? DateTime.Now
            };
        }
    }

    extension(TyphoonForecastResponse.TyphoonForecastItem item)
    {
        public TyphoonTrackBase MapToQTyphoonTrack()
        {
            return new TyphoonTrackBase
            {
                Latitude = item.Lat,
                Longitude = item.Lon,
                MoveSpeed = item.MoveSpeed,
                WindSpeed = item.WindSpeed,
                Pressure = item.Pressure,
                Type = (TyphoonType)Enum.Parse(typeof(TyphoonType), item.Type),
                Time = item.FxTime,
            };
        }
    }

    extension(WindRadiusItem item)
    {
        public WindRadius MapToWindRadius()
        {
            return new WindRadius
            {
                SouthEast = item.SeRadius,
                SouthWest = item.SwRadius,
                NorthEast = item.NeRadius,
                NorthWest = item.NwRadius,
            };
        }
    }
}
