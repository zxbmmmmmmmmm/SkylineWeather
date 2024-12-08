using SkylineWeather.DataAnalyzer.Models;
using UnitsNet;

namespace SkylineWeather.DataAnalyzer;

public class TemperatureTrendAnalyzer : ITrendAnalyzer<Temperature,TemperatureTrend>
{
    public TemperatureTrend GetTrend(IEnumerable<Temperature> data)
    {
        //使用最小二乘法计算温度趋势
        var x = data.Select((t, i) => (double)i).ToArray();
        var y = data.Select(t => t.DegreesCelsius).ToArray();
        var n = x.Length;
        var sumX = x.Sum();
        var sumY = y.Sum();
        var sumX2 = x.Select(xx => xx * xx).Sum();
        var sumY2 = y.Select(yy => yy * yy).Sum();
        var sumXY = x.Zip(y, (xx, yy) => xx * yy).Sum();
        var a = (sumY * sumX2 - sumX * sumXY) / (n * sumX2 - sumX * sumX);
        var b = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        var r = (n * sumXY - sumX * sumY) / Math.Sqrt((n * sumX2 - sumX * sumX) * (n * sumY2 - sumY * sumY));
        var trend = new TemperatureTrend
        {
            Slope = b,
            Intercept = a,
            CorrelationCoefficient = r
        };

        if (Math.Abs(trend.CorrelationCoefficient) <= 0.7)
        {
            trend.Type = TemperatureTrendType.Fluctuating;
        }
        else
        {
            trend.Type = trend.Slope switch
            {
                >= 0.5 => TemperatureTrendType.Increasing,
                <= -0.5 => TemperatureTrendType.Decreasing,
                _ => TemperatureTrendType.Steady
            };
        }

        return trend;
    }
}