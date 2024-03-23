using System;
using System.Net;
using System.Text.Json.Serialization;

namespace FluentWeather.QWeatherApi.Bases;

public abstract class QWeatherResponseBase
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("updateTime")]
    public string UpdateTime { get; set; }

    [JsonPropertyName("fxLink")]
    public string FxLink{ get; set; }
}