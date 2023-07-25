using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentWeather.QWeatherApi;

public class ApiHandlerOption
{
    public Dictionary<string, string> Cookies { get; } = new();
    public string Token { get; set; } 

    public JsonSerializerOptions JsonSerializerOptions =
        new(JsonSerializerOptions.Default)
        {
            NumberHandling = JsonNumberHandling.WriteAsString |
                             JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
        };
}