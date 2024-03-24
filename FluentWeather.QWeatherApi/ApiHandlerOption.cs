using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QWeatherApi;

public class ApiHandlerOption
{
    public Dictionary<string, string> Cookies { get; } = new();
    public string Token { get; set; } 
    public string Language { get; set; }
    public string PublicId { get; set; }

    public JsonSerializerOptions JsonSerializerOptions =
        new(JsonSerializerOptions.Default)
        {
            NumberHandling = JsonNumberHandling.WriteAsString |
                             JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
        };
    public string Domain{ get; set; }
}