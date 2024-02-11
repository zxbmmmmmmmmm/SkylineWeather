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
            Latitude = double.Parse(item.Lat),
            Longitude = double.Parse(item.Lon),
            MoveSpeed = int.Parse(item.MoveSpeed),
            WindSpeed = int.Parse(item.WindSpeed),
            Pressure = int.Parse(item.Pressure),
            WindRadius7 = item.WindRadius7?.MapToWindRadius(),
            WindRadius10 = item.WindRadius10?.MapToWindRadius(),
            WindRadius12 = item.WindRadius12?.MapToWindRadius(),
            Type = (TyphoonType)Enum.Parse(typeof(TyphoonType), item.Type),
            Time = (item.Time is null)? DateTime.Now: DateTime.Parse(item.Time)
        };
    }
    public static TyphoonTrackBase MapToQTyphoonTrack(this TyphoonForecastResponse.TyphoonForecastItem item)
    {
        return new TyphoonTrackBase
        {
            Latitude = double.Parse(item.Lat),
            Longitude = double.Parse(item.Lon),
            MoveSpeed = (item.MoveSpeed is "") ? null : int.Parse(item.MoveSpeed),
            WindSpeed = int.Parse(item.WindSpeed),
            Pressure = int.Parse(item.Pressure),
            Type = (TyphoonType)Enum.Parse(typeof(TyphoonType), item.Type),
            Time = DateTime.Parse(item.FxTime),
        };
    }
    public static WindRadius MapToWindRadius(this WindRadiusItem item)
    {
        return new WindRadius
        {
            SouthEast = int.Parse(item.SeRadius),
            SouthWest = int.Parse(item.SwRadius),
            NorthEast = int.Parse(item.NeRadius),
            NorthWest = int.Parse(item.NwRadius),
        };
    }
}
