namespace SkylineWeather.DataAnalyzer.Models;

public interface ITrendAnalyzer<in TInput, out TResult> where TResult : Trend
{
    public TResult GetTrend(IEnumerable<TInput> data);
}