using SkylineWeather.Abstractions.Models.AirQuality;
using System.Collections.Generic;

namespace SkylineWeather.DataAnalyzer.Analyzers;

public class ChinaAqiAnalyzer : AqiAnalyzer
{
    public override AqiStandard Standard => AqiStandard.China;

    private static readonly (int aqi, AqiLevelDescriptor level)[] AqiLevels =
    [
        (0, new AqiLevelDescriptor
        {
            Level = 1,
            Category = "优",
            HealthImplication = "空气质量令人满意，基本无空气污染。"
        }),
        (51, new AqiLevelDescriptor
        {
            Level = 2, 
            Category = "良", 
            HealthImplication = "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响。" 
        }),
        (101, new AqiLevelDescriptor
        { 
            Level = 3, 
            Category = "轻度污染",
            HealthImplication = "易感人群症状有轻度加剧，健康人群出现刺激症状。" 
        }),
        (151, new AqiLevelDescriptor
        {
            Level = 4,
            Category = "中度污染", 
            HealthImplication = "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状。" 
        }),
        (201, new AqiLevelDescriptor
        { 
            Level = 5, 
            Category = "重度污染",
            HealthImplication = "心脏病和肺病患者运动耐受力降低，健康人群中普遍出现症状。" 
        }),
        (301, new AqiLevelDescriptor
        { 
            Level = 6, 
            Category = "严重污染", 
            HealthImplication = "健康人运动耐受力降低，有明显强烈症状，提前出现某些疾病。" 
        })
    ];

    private static readonly Dictionary<PollutantType, PollutantBreakpoint[]> Thresholds = new()
    {
        // PM2.5 (μg/m³) - 24-hour
        [PollutantType.PM25] =
        [
            new(0, 0), new(35, 50), new(75, 100), new(115, 150),
            new(150, 200), new(250, 300), new(350, 400), new(500, 500)
        ],
        // PM10 (μg/m³) - 24-hour
        [PollutantType.PM10] =
        [
            new(0, 0), new(50, 50), new(150, 100), new(250, 150),
            new(350, 200), new(420, 300), new(500, 400), new(600, 500)
        ],
        // O3 (μg/m³) - 8-hour
        [PollutantType.O3] =
        [
            new(0, 0), new(160, 50), new(200, 100), new(300, 150),
            new(400, 200), new(800, 300), new(1000, 400), new(1200, 500)
        ],
        // CO (mg/m³) - 24-hour
        [PollutantType.CO] =
        [
            new(0, 0), new(5, 50), new(10, 100), new(35, 150),
            new(60, 200), new(90, 300), new(120, 400), new(150, 500)
        ],
        // SO2 (μg/m³) - 24-hour
        [PollutantType.SO2] =
        [
            new(0, 0), new(150, 50), new(500, 100), new(650, 150),
            new(800, 200), new(1600, 300), new(2100, 400), new(2620, 500)
        ],
        // NO2 (μg/m³) - 24-hour
        [PollutantType.NO2] =
        [
            new(0, 0), new(100, 50), new(200, 100), new(700, 150),
            new(1200, 200), new(2340, 300), new(3090, 400), new(3840, 500)
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