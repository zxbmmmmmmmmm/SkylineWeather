using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;
using System.Runtime.CompilerServices;
using static FluentWeather.QWeatherApi.ApiContracts.WeatherIndicesResponse;

namespace FluentWeather.QWeatherProvider.Mappers;

public static class WeatherIndicesMapper
{
    public static QWeatherIndices MapToQWeatherIndices(this IndicesItem item)
    {
        return new QWeatherIndices
        {
            Category = item.Category,
            Date = DateTime.Parse(item.Date),
            Name = item.Name,
            Description = item.Text,
            Level = int.Parse(item.Level),
            Type = int.Parse(item.Type)
        };
    }
}