namespace FluentWeather.Abstraction.Models;
/// <summary>
/// 生活指数
/// </summary>
public class IndicesBase
{
    public  string Name { get; set; }
    public  string Category{ get; set; }
    public string? Description { get; set; }
}