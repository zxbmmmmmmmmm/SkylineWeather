using OpenMeteoApi.Models;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.AirQuality;
using UnitsNet;
using UnitsNet.Units;

namespace OpenMeteoProvider.Mappers;

public static class AirConditionMapper
{
    public static AirQuality MapToAirQuality(this AirQualityData data)
    {
        var current = data.Current;
        return new AirQuality
        {
            PM25 = new Density((double)current!.Pm25!, DensityUnit.MicrogramPerCubicMeter),
            PM10 = new Density((double)current.Pm10!, DensityUnit.MicrogramPerCubicMeter),
            SO2 = new Density((double)current.SulphurDioxide!, DensityUnit.MicrogramPerCubicMeter),
            NO2 = new Density((double)current.NitrogenDioxide!, DensityUnit.MicrogramPerCubicMeter),
            O3 = new Density((double)current.Ozone!, DensityUnit.MicrogramPerCubicMeter),
            CO = new Density((double)current.CarbonMonoxide!, DensityUnit.MicrogramPerCubicMeter),
            Aqi = new Aqi
            {
                Value = (double)current.UsAqi!,
                Standard = AqiStandard.Usa,
            }
        };
    }
}