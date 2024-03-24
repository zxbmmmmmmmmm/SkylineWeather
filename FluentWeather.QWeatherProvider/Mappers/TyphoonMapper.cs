using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static FluentWeather.QWeatherApi.ApiContracts.TyphoonTrackItem;

namespace FluentWeather.QWeatherProvider.Mappers;
public static class TyphoonMapper
{
    public static TyphoonBase MapToTyphoonBase(this TyphoonTrackResponse item,string name)
    {
        return new TyphoonBase
        {
            Name = name,
            IsActive = item.IsActive is "1",
            History = item.Tracks.ConvertAll(p => (TyphoonTrackBase)p.MapToQTyphoonTrack()),
            Now = item.Now.MapToQTyphoonTrack(),
        };
    }
    public static QTyphoonTrack MapToQTyphoonTrack(this TyphoonTrackItem item)
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
    public static TyphoonTrackBase MapToQTyphoonTrack(this TyphoonForecastResponse.TyphoonForecastItem item)
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
    public static WindRadius MapToWindRadius(this WindRadiusItem item)
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
