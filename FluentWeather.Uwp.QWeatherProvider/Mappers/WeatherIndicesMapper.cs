using FluentWeather.Uwp.QWeatherProvider.Models;
using static QWeatherApi.ApiContracts.WeatherIndicesResponse;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers;

public static class WeatherIndicesMapper
{
    extension(IndicesItem item)
    {
        public QWeatherIndices MapToQWeatherIndices()
        {
            return new QWeatherIndices
            {
                Category = item.Category,
                Date = item.Date,
                Name = item.Name,
                Description = item.Text,
                Level = item.Level,
                Type = item.Type
            };
        }
    }
}