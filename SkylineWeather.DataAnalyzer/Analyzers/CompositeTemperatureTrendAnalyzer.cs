using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;

namespace SkylineWeather.DataAnalyzer.Analyzers;

public class CompositeTemperatureTrendAnalyzer : ITrendAnalyzer<(Temperature min, Temperature max), TemperatureTrend>
{
    private const double SignificantSlope = 0.5;
    private const double WeakCorrelation = 0.7;
    public static CompositeTemperatureTrendAnalyzer Instance { get; } = new();
    public TemperatureTrend GetTrend(IEnumerable<(Temperature min, Temperature max)> data)
    {
        var dailyData = data.ToArray();
        if (dailyData.Length < 2)
        {
            return new TemperatureTrend { Type = TemperatureTrendType.Steady };
        }

        // 1. 分别提取最高温和最低温序列
        var maxTemps = dailyData.Select(d => d.max);
        var minTemps = dailyData.Select(d => d.min);

        // 2. 调用您的算法，分别分析最高温和最低温的趋势
        var maxTrend = SingleTemperatureTrendAnalyzer.Instance.GetTrend(maxTemps);
        var minTrend = SingleTemperatureTrendAnalyzer.Instance.GetTrend(minTemps);

        TemperatureTrendType finalTrendType;

        // 3. 组合判断逻辑
        // 规则 1: 使用相关系数判断波动。如果任一序列的线性关系不强，说明数据点很散乱，整体表现为波动。
        if (Math.Abs(maxTrend.CorrelationCoefficient) < WeakCorrelation ||
            Math.Abs(minTrend.CorrelationCoefficient) < WeakCorrelation)
        {
            finalTrendType = TemperatureTrendType.Fluctuating;
        }
        // 规则 2: 判断温差拉大的情况 (最典型的波动)
        else if (maxTrend.Slope > SignificantSlope && minTrend.Slope < -SignificantSlope)
        {
            finalTrendType = TemperatureTrendType.Fluctuating;
        }
        // 规则 3: 判断明确的升高趋势
        else if (maxTrend.Slope > SignificantSlope && minTrend.Slope > SignificantSlope)
        {
            finalTrendType = TemperatureTrendType.Increasing;
        }
        // 规则 4: 判断明确的下降趋势
        else if (maxTrend.Slope < -SignificantSlope && minTrend.Slope < -SignificantSlope)
        {
            finalTrendType = TemperatureTrendType.Decreasing;
        }
        // 规则 5: 其他情况（如一个平稳一个上升/下降，或两者都平稳）均视为整体平稳
        else
        {
            finalTrendType = TemperatureTrendType.Steady;
        }

        // 4. 计算平均温度的趋势，用于填充返回对象的统计值
        var avgTemps = dailyData.Select(d => Temperature.FromDegreesCelsius((d.min.DegreesCelsius + d.max.DegreesCelsius) / 2.0));
        var avgTrend = SingleTemperatureTrendAnalyzer.Instance.GetTrend(avgTemps);

        // 5. 返回最终结果
        return new TemperatureTrend
        {
            Type = finalTrendType, // 使用我们组合逻辑判断出的类型
            Slope = avgTrend.Slope, // 使用平均值的斜率作为代表
            Intercept = avgTrend.Intercept,
            CorrelationCoefficient = avgTrend.CorrelationCoefficient
        };
    }
}