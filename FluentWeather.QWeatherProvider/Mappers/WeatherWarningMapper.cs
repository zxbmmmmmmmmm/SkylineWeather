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
            Description = item.Text,
            Sender = item.Sender,
            Severity = item.Severity,
            SeverityColor = (SeverityColor)Enum.Parse(typeof(SeverityColor),item.SeverityColor),//可能出问题
            StartTime = DateTime.Parse(item.StartTime),
            EndTime = DateTime.Parse(item.EndTime),
            PublishTime = DateTime.Parse(item.PubTime),
            Title = item.Title,
            WarningType = item.Type,
        };
    }
}