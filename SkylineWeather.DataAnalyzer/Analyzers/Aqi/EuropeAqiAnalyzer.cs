using SkylineWeather.Abstractions.Models.AirQuality;
using System.Collections.Generic;

namespace SkylineWeather.DataAnalyzer.Analyzers;

/// <summary>
/// Implements the European Common Air Quality Index (CAQI).
/// The CAQI is an index from 0 to >100, divided into five levels.
/// The final index is the maximum of the sub-indices for the individual pollutants.
/// </summary>
public class EuropeAqiAnalyzer : AqiAnalyzer
{
    public override AqiStandard Standard => AqiStandard.Europe;

    private static readonly (int aqi, AqiLevelDescriptor level)[] AqiLevels =
    [
        (0, new AqiLevelDescriptor
        {
            Level = 1,
            Category = "Very Low",
            HealthImplication = "Air quality is very good."
        }),
        (25, new AqiLevelDescriptor
        {
            Level = 2,
            Category = "Low",
            HealthImplication = "Air quality is good."
        }),
        (50, new AqiLevelDescriptor
        {
            Level = 3,
            Category = "Medium",
            HealthImplication = "Air quality is fair."
        }),
        (75, new AqiLevelDescriptor
        {
            Level = 4,
            Category = "High",
            HealthImplication = "Air quality is poor."
        }),
        (100, new AqiLevelDescriptor
        {
            Level = 5,
            Category = "Very High",
            HealthImplication = "Air quality is very poor."
        })
    ];

    private static readonly Dictionary<PollutantType, PollutantBreakpoint[]> Thresholds = new()
    {
        // PM2.5 (μg/m³) - hourly
        [PollutantType.PM25] =
        [
            new(0, 0), new(15, 25), new(30, 50), new(55, 75), new(110, 100)
        ],
        // PM10 (μg/m³) - hourly
        [PollutantType.PM10] =
        [
            new(0, 0), new(25, 25), new(50, 50), new(90, 75), new(180, 100)
        ],
        // O3 (μg/m³) - hourly
        [PollutantType.O3] =
        [
            new(0, 0), new(60, 25), new(120, 50), new(180, 75), new(240, 100)
        ],
        // CO (mg/m³) - hourly
        [PollutantType.CO] =
        [
            new(0, 0), new(5, 25), new(7.5, 50), new(10, 75), new(20, 100)
        ],
        // SO2 (μg/m³) - hourly
        [PollutantType.SO2] =
        [
            new(0, 0), new(50, 25), new(100, 50), new(350, 75), new(500, 100)
        ],
        // NO2 (μg/m³) - hourly
        [PollutantType.NO2] =
        [
            new(0, 0), new(50, 25), new(100, 50), new(200, 75), new(400, 100)
        ]
    };

    protected override Dictionary<PollutantType, PollutantBreakpoint[]> GetPollutantThresholds()
    {
        return Thresholds;
    }

    protected override (int aqi, AqiLevelDescriptor level)[] GetAqiLevels()
    {
        return AqiLevels;
    }
}