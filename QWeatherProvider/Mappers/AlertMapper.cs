using QWeatherApi.ApiContracts;
using SkylineWeather.Abstractions.Models.Alert;

namespace QWeatherProvider.Mappers;

public static class AlertMapper
{
    public static Alert MapToAlert(this WeatherWarningResponse.WeatherWarningItem warning)
    {
        return new Alert()
        {
            Title = warning.Title,
            Description = warning.Text,
            Sender = warning.Sender,
            StartTime = warning.StartTime,
            EndTime = warning.EndTime,
            PublishTime = warning.PubTime,
            SeverityColor = warning.SeverityColor is not "" ? (SeverityColor)Enum.Parse(typeof(SeverityColor), warning.SeverityColor) : null,
        };
    }
}