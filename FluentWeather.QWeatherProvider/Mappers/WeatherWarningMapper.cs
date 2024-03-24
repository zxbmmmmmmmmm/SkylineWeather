using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;

namespace FluentWeather.QWeatherProvider.Mappers;

public static class WeatherWarningMapper
{
    public static WeatherWarningBase MapToWeatherWarningBase(this WeatherWarningResponse.WeatherWarningItem item)
    {
        return new WeatherWarningBase
        {
            Id = item.Id,
            Description = item.Text,
            Sender = item.Sender,
            Severity = item.Severity,
            SeverityColor = item.SeverityColor is not "" ?(SeverityColor)Enum.Parse(typeof(SeverityColor),item.SeverityColor) : null,
            StartTime = item.StartTime,
            EndTime = item.EndTime,
            PublishTime = item.PubTime,
            Title = item.Title,
            WarningType = item.Type,
        };
    }
}