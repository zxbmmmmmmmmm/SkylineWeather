namespace FluentWeather.Abstraction.Models;
/// <summary>
/// 生活指数
/// </summary>
public class IndicesBase
{
    public required string Name { get; set; }
    public required string Category{ get; set; }
    public string? Description { get; set; }
}