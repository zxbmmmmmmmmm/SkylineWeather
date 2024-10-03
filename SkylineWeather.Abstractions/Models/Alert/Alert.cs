namespace SkylineWeather.Abstractions.Models.Alert;

public class Alert
{
    public required string Title {  get; set; }
    public required string Description { get; set; }
    public string? Sender { get; set; }
    public AlertStatus? Status { get; set; }
    public DateTimeOffset? PublishTime { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public SeverityColor? SeverityColor { get; set; }
}