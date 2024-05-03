using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherWarningBase
{
    public string? Id{ get; set; }
    public string? Sender { get; set; }
    public  DateTime PublishTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? WarningType { get; set; }
    public string? Severity { get; set; }
    public SeverityColor? SeverityColor { get; set; }
    public virtual string? ShortTitle
    {
        get
        {
            var index = Title?.IndexOf("发布");
            if (index is null or 0) return Title;
            index += 2;
            return Title?.Substring(index.Value);
        }
    } 
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