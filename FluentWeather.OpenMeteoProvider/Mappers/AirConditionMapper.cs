using FluentWeather.Abstraction.Models;
using FluentWeather.OpenMeteoProvider.Models;
using OpenMeteoApi.Models;
using System;
using System.Collections.Generic;

namespace FluentWeather.OpenMeteoProvider.Mappers;

public static class AirConditionMapper
{
    extension(AirQualityData data)
    {
        public OpenMeteoAirCondition MapToOpenMeteoWeatherNow()
        {
            var current = data.Current;
            var airCondition = new OpenMeteoAirCondition
            {
                Aqi = (int)current!.UsAqi!,//使用UsAqi
                Pollutants = CreatePollutants(data),
            };
            airCondition.AqiLevel = airCondition.Aqi switch
            {
                <= 50 => 0,
                <= 100 => 1,
                <= 150 => 2,
                <= 200 => 3,
                <= 300 => 4,
                _ => 5,
            };
            return airCondition;
        }
    }

    private static List<Pollutant> CreatePollutants(AirQualityData data)
    {
        var current = data.Current;
        var units = data.CurrentUnits;
        List<Pollutant> pollutants = new();
        AddPollutant(pollutants, "pm2p5", "PM2.5", "Particulate Matter 2.5", current?.Pm25, units?.Pm25);
        AddPollutant(pollutants, "pm10", "PM10", "Particulate Matter 10", current?.Pm10, units?.Pm10);
        AddPollutant(pollutants, "so2", "SO₂", "Sulphur Dioxide", current?.SulphurDioxide, units?.SulphurDioxide);
        AddPollutant(pollutants, "no2", "NO₂", "Nitrogen Dioxide", current?.NitrogenDioxide, units?.NitrogenDioxide);
        AddPollutant(pollutants, "o3", "O₃", "Ozone", current?.Ozone, units?.Ozone);
        AddPollutant(pollutants, "co", "CO", "Carbon Monoxide", current?.CarbonMonoxide, units?.CarbonMonoxide);
        AddPollutant(pollutants, "nh3", "NH₃", "Ammonia", current?.Ammonia, units?.Ammonia);
        return pollutants;
    }

    private static void AddPollutant(List<Pollutant> pollutants, string code, string name, string fullName, float? value, string? unit)
    {
        if (!value.HasValue)
        {
            return;
        }

        pollutants.Add(new Pollutant
        {
            Code = code,
            Name = name,
            FullName = fullName,
            Unit = unit,
            Value = Math.Round(value.Value, 2),
        });
    }
}