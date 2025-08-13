namespace SkylineWeather.Abstractions.Models.AirQuality;

/// <summary>
/// 描述一个特定的空气质量指数 (AQI) 等级
/// </summary>
public record AqiLevelDescriptor
{
    public required int Level { get; init; }

    /// <summary>
    /// 等级的名称（例如，“优”、“中度污染”）。
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    /// 对健康影响的详细描述。
    /// </summary>
    public string? HealthImplication { get; init; }

    /// <summary>
    /// 针对该空气质量等级的建议措施。
    /// </summary>
    public string? RecommendedAction { get; init; }
}