using SkylineWeather.Abstractions.Models.AirQuality;
using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;
using UnitsNet.Units;

namespace SkylineWeather.DataAnalyzer.Analyzers;

public abstract class AqiAnalyzer : IAqiAnalyzer
{
    // 定义污染物浓度和对应的AQI值的分级
    protected record PollutantBreakpoint(double Concentration, int Aqi);

    public abstract AqiStandard Standard { get; }

    // 要求子类提供各自的污染物浓度阈值
    protected abstract Dictionary<PollutantType, PollutantBreakpoint[]> GetPollutantThresholds();

    // 要求子类提供各自的AQI等级定义
    protected abstract (int aqi, AqiLevelDescriptor level)[] GetAqiLevels();

    public Aqi CalculateAqi(AirQuality airQuality)
    {
        var pollutantThresholds = GetPollutantThresholds();

        var individualAqis = new List<double>();
        var aqi = new Aqi
        {
            Value = 0,
            Standard = Standard,
            LevelDescriptor = GetAqiLevel(0)
        }; ;
        // 计算各种污染物的IAQI

        aqi.SubAqis.PM25 = AddIndividualAqi(airQuality.PM25, PollutantType.PM25);
        aqi.SubAqis.PM10 = AddIndividualAqi(airQuality.PM10, PollutantType.PM10);
        aqi.SubAqis.O3 = AddIndividualAqi(airQuality.O3, PollutantType.O3);
        aqi.SubAqis.CO = AddIndividualAqi(airQuality.CO, PollutantType.CO);
        aqi.SubAqis.SO2 = AddIndividualAqi(airQuality.SO2, PollutantType.SO2);
        aqi.SubAqis.NO2 = AddIndividualAqi(airQuality.NO2, PollutantType.NO2);

        if (individualAqis.Count == 0)
        {
            return aqi;
        }

        // 最终AQI取各项污染物的最大值
        var finalAqi = individualAqis.Max();
        var level = GetAqiLevel((int)finalAqi);

        aqi.Value = finalAqi;
        aqi.Standard = Standard;
        aqi.LevelDescriptor = level;
        return aqi;

        double? AddIndividualAqi(Density? density, PollutantType type)
        {
            if (density.HasValue && pollutantThresholds.TryGetValue(type, out var thresholds))
            {
                var concentration = GetConcentrationInStandardUnits(density.Value, type);
                var individualAqi = CalculateIndividualAqi(concentration, thresholds);
                individualAqis.Add(individualAqi);
                return individualAqi;
            }
            return null;
        }
    }

    private static double CalculateIndividualAqi(double concentration, PollutantBreakpoint[] breakpoints)
    {
        if (concentration > breakpoints.Last().Concentration)
        {
            // 如果浓度超过最高阈值，则返回最高AQI值（通常为500）
            return 500;
        }

        var high = breakpoints.First(b => b.Concentration >= concentration);
        var low = breakpoints.Last(b => b.Concentration <= concentration);

        if (high == low)
        {
            return high.Aqi;
        }

        // 使用线性插值公式计算IAQI
        var iaqi = (high.Aqi - low.Aqi) / (high.Concentration - low.Concentration) * (concentration - low.Concentration) + low.Aqi;
        return Math.Round(iaqi);
    }

    private AqiLevelDescriptor GetAqiLevel(int aqi)
    {
        var aqiLevels = GetAqiLevels();
        return aqiLevels.Last(l => aqi >= l.aqi).level;
    }

    // 将不同单位的浓度统一转换为计算所需的单位
    private static double GetConcentrationInStandardUnits(Density density, PollutantType type)
    {
        return type switch
        {
            PollutantType.CO => density.ToUnit(DensityUnit.MilligramPerCubicMeter).Value,
            _ => density.ToUnit(DensityUnit.MicrogramPerCubicMeter).Value,
        };
    }

    protected enum PollutantType { PM25, PM10, O3, CO, SO2, NO2 }
}