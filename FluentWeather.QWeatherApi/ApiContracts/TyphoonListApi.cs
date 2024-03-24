using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using QWeatherApi.Bases;

namespace QWeatherApi.ApiContracts;

public sealed class TyphoonListApi : QApiContractBase<RequestBase,TyphoonListResponse>
{
    public override HttpMethod Method => HttpMethod.Get;

    public override string Path => ApiConstants.Weather.TyphoonList;
    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var res = await base.GenerateRequestMessageAsync(option);
        var uri = res.RequestUri.ToString();
        uri += $"&basin=NP&year={DateTime.Now.Year}";
        res.RequestUri = new Uri(uri);
        return res;
    }
}
public sealed class TyphoonListResponse : QWeatherResponseBase
{
    [JsonPropertyName("storm")]
    public List<TyphoonListItem> Typhoons { get; set; }
    public class TyphoonListItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("basin")]
        public string Basin { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        [JsonPropertyName("isActive")]
        public string IsActive { get; set; }
    }
}
