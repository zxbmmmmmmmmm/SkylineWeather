using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherProvider.Models;
using System;

namespace FluentWeather.QWeatherProvider.Mappers;

public static class WeatherWarningMapper
{
    public static QWeatherWarning MapToQWeatherWarning(this WeatherWarningResponse.WeatherWarningItem item)
    {
        return new QWeatherWarning
        {
            Id = item.Id,
            Description = item.Text,
            Sender = item.Sender,
            Severity = item.Severity,
            SeverityColor = item.SeverityColor is not "" ?(SeverityColor)Enum.Parse(typeof(SeverityColor),item.SeverityColor) : null,
            StartTime = DateTime.Parse(item.StartTime),
            EndTime = DateTime.Parse(item.EndTime),
            PublishTime = DateTime.Parse(item.PubTime),
            Title = item.Title,
            WarningType = item.Type,
        };
    }
}