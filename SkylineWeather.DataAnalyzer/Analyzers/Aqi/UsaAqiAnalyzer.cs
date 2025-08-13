using SkylineWeather.Abstractions.Models.AirQuality;
using System.Collections.Generic;

namespace SkylineWeather.DataAnalyzer.Analyzers;

public class UsaAqiAnalyzer : AqiAnalyzer
{
    public override AqiStandard Standard => AqiStandard.Usa;

    private static readonly (int aqi, AqiLevelDescriptor level)[] AqiLevels =
    [
        (0, new AqiLevelDescriptor
        {
            Level = 1,
            Category = "Good",
            HealthImplication = "Air quality is satisfactory, and air pollution poses little or no risk." 
        }),
        (51, new AqiLevelDescriptor
        {
            Level = 2,
            Category = "Moderate",
            HealthImplication = "Air quality is acceptable. However, there may be a risk for some people, particularly those who are unusually sensitive to air pollution." 
        }),
        (101, new AqiLevelDescriptor
        {
            Level = 3, 
            Category = "Unhealthy for Sensitive Groups", 
            HealthImplication = "Members of sensitive groups may experience health effects. The general public is less likely to be affected." 
        }),
        (151, new AqiLevelDescriptor
        {
            Level = 4, 
            Category = "Unhealthy", 
            HealthImplication = "Some members of the general public may experience health effects; members of sensitive groups may experience more serious health effects." 
        }),
        (201, new AqiLevelDescriptor
        {
            Level = 5, 
            Category = "Very Unhealthy",
            HealthImplication = "Health alert: The risk of health effects is increased for everyone."
        }),
        (301, new AqiLevelDescriptor
        {
            Level = 6, 
            Category = "Hazardous",
            HealthImplication = "Health warning of emergency conditions: everyone is more likely to be affected." 
        })
    ];

    private static readonly Dictionary<PollutantType, PollutantBreakpoint[]> Thresholds = new()
    {
        // PM2.5 (μg/m³) - 24-hour
        [PollutantType.PM25] =
        [
            new(0.0, 0), new(12.0, 50), new(35.4, 100), new(55.4, 150),
            new(150.4, 200), new(250.4, 300), new(350.4, 400), new(500.4, 500)
        ],
        // PM10 (μg/m³) - 24-hour
        [PollutantType.PM10] =
        [
            new(0, 0), new(54, 50), new(154, 100), new(254, 150),
            new(354, 200), new(424, 300), new(504, 400), new(604, 500)
        ],
        // O3 (μg/m³) - 8-hour. Note: EPA uses ppb, conversion is needed. 1 ppb O3 ≈ 1.96 μg/m³
        [PollutantType.O3] =
        [
            new(0, 0), new(106, 50), // 54 ppb * 1.96
            new(137, 100), // 70 ppb * 1.96
            new(167, 150), // 85 ppb * 1.96
            new(204, 200), // 104 ppb * 1.96
            new(392, 300)  // 200 ppb * 1.96
        ],
        // CO (mg/m³) - 8-hour. Note: EPA uses ppm, conversion is needed. 1 ppm CO ≈ 1.145 mg/m³
        [PollutantType.CO] =
        [
            new(0.0, 0), new(5.0, 50),   // 4.4 ppm * 1.145
            new(10.8, 100), // 9.4 ppm * 1.145
            new(14.2, 150), // 12.4 ppm * 1.145
            new(17.7, 200), // 15.4 ppm * 1.145
            new(34.8, 300), // 30.4 ppm * 1.145
            new(46.3, 400), // 40.4 ppm * 1.145
            new(57.7, 500)  // 50.4 ppm * 1.145
        ],
        // SO2 (μg/m³) - 1-hour. Note: EPA uses ppb, conversion is needed. 1 ppb SO2 ≈ 2.62 μg/m³
        [PollutantType.SO2] =
        [
            new(0, 0), new(92, 50),    // 35 ppb * 2.62
            new(196, 100),   // 75 ppb * 2.62
            new(489, 150),   // 185 ppb * 2.62
            new(804, 200),   // 304 ppb * 2.62
            new(1600, 300),  // 604 ppb * 2.62
            new(2100, 400),  // 804 ppb * 2.62
            new(2620, 500)   // 1004 ppb * 2.62
        ],
        // NO2 (μg/m³) - 1-hour. Note: EPA uses ppb, conversion is needed. 1 ppb NO2 ≈ 1.88 μg/m³
        [PollutantType.NO2] =
        [
            new(0, 0), new(100, 50),   // 53 ppb * 1.88
            new(188, 100),  // 100 ppb * 1.88
            new(677, 150),  // 360 ppb * 1.88
            new(1203, 200), // 640 ppb * 1.88
            new(2330, 300), // 1240 ppb * 1.88
            new(3083, 400), // 1640 ppb * 1.88
            new(3816, 500)  // 2040 ppb * 1.88
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