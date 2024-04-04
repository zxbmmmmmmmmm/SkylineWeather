using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers
{
    internal static class AirConditionMapper
    {
        public static QAirCondition MapToQAirCondition(this AirConditionResponse.AirConditionItem item)
        {
            return new QAirCondition
            {
                Aqi = item.Aqi,
                AqiCategory = item.Category,
                AqiLevel = item.Level,
                CO= item.Co,
                SO2 = item.So2,
                NO2 = item.No2,
                O3 = item.O3,
                PM10 = item.Pm10,
                PM25 = item.Pm2p5,
            };
        }
    }
}
