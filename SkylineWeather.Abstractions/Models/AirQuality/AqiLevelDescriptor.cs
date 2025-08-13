namespace SkylineWeather.Abstractions.Models.AirQuality;

/// <summary>
/// ����һ���ض��Ŀ�������ָ�� (AQI) �ȼ�
/// </summary>
public record AqiLevelDescriptor
{
    public required int Level { get; init; }

    /// <summary>
    /// �ȼ������ƣ����磬���š������ж���Ⱦ������
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    /// �Խ���Ӱ�����ϸ������
    /// </summary>
    public string? HealthImplication { get; init; }

    /// <summary>
    /// ��Ըÿ��������ȼ��Ľ����ʩ��
    /// </summary>
    public string? RecommendedAction { get; init; }
}