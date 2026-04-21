using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.QWeatherProvider.Models;
using QWeatherApi.ApiContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentWeather.Uwp.QWeatherProvider.Mappers
{
    internal static class AirConditionMapper
    {
        extension(AirConditionResponse item)
        {
            public QAirCondition MapToQAirCondition()
            {
                var index = item.Indexes?.FirstOrDefault();
                var airCondition = new QAirCondition
                {
                    Aqi = index is null ? 0 : (int)Math.Round(index.Aqi),
                    AqiCategory = index?.Category,
                    AqiLevel = GetAqiLevel(index?.Level),
                    Pollutants = item.Pollutants?.ConvertAll(pollutant => new Pollutant
                    {
                        Code = pollutant.Code,
                        Name = string.IsNullOrWhiteSpace(pollutant.Name) ? pollutant.FullName : pollutant.Name,
                        FullName = pollutant.FullName,
                        Unit = pollutant.Concentration.Unit,
                        Value = pollutant.Concentration.Value,
                    }) ?? new List<Pollutant>(),
                };
                return airCondition;
            }
        }

        private static int? GetAqiLevel(string level)
        {
            if (!int.TryParse(level, out var parsedLevel))
            {
                return null;
            }

            return Math.Max(parsedLevel - 1, 0);
        }
    }
}
