using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherWarningBase
{
    public string? Id{ get; set; }
    public string? Sender { get; set; }
    public required DateTime PublishTime { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? WarningType { get; set; }
    public string? Severity { get; set; }
    public SeverityColor? SeverityColor { get; set; }
}
public enum SeverityColor
{
    White,
    Blue,
    Green,
    Yellow,
    Orange,
    Red,
    Black,
}