using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentWeather.QWeatherApi.Bases;

namespace FluentWeather.QWeatherApi.ApiContracts;

public sealed class WeatherIndicesApi : QApiContractBase<WeatherIndicesResponse>
{
    public override HttpMethod Method => HttpMethod.Get;
    public override string Path => ApiConstants.Weather.WeatherIndices1D;

    protected override NameValueCollection GenerateQuery(ApiHandlerOption option)
    {
        var result = base.GenerateQuery(option);
        result.Add("type", "0");
        return result;
    }
}
public sealed class WeatherIndicesResponse : QWeatherResponseBase
{
    [JsonPropertyName("daily")] public List<IndicesItem> Indices { get; set; }
    public class IndicesItem
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

}