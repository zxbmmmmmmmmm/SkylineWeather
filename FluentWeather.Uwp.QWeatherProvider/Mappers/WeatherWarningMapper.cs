using System;
using FluentWeather.Abstraction.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class WeatherWarningMapper
{
    extension(WeatherWarningResponse.WeatherWarningItem item)
    {
        public WeatherWarningBase MapToWeatherWarningBase()
        {
            return new WeatherWarningBase
            {
                Id = item.Id,
                Description = item.Text,
                Sender = item.Sender,
                Severity = item.Severity,
                SeverityColor = item.SeverityColor is not "" ? (SeverityColor)Enum.Parse(typeof(SeverityColor), item.SeverityColor) : null,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                PublishTime = item.PubTime,
                Title = item.Title,
                WarningType = item.Type,
            };
        }
    }
}