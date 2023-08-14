using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentWeather.QWeatherApi.Bases;

namespace FluentWeather.QWeatherApi.ApiContracts;

public class WeatherIndicesApi : QApiContractBase<WeatherIndicesResponse>
{
    public override HttpMethod Method => HttpMethod.Get;
    public override string Path => ApiConstants.Weather.WeatherIndices1D;
    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var res = await base.GenerateRequestMessageAsync(option);
        var uri = res.RequestUri.ToString();
        uri += "&type=0";
        res.RequestUri = new System.Uri(uri);
        return res;
    }
}
public class WeatherIndicesResponse : QWeatherResponseBase
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