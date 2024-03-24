using System.Text.Json.Serialization;

namespace QWeatherApi.Bases;

public abstract class QWeatherResponseBase
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("updateTime")]
    public string UpdateTime { get; set; }

    [JsonPropertyName("fxLink")]
    public string FxLink{ get; set; }
}