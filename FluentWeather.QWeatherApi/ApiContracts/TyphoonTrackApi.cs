using FluentWeather.QWeatherApi.Bases;
using FluentWeather.QWeatherApi.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherApi.ApiContracts;

/// <summary>
/// 台风当前情况和历史
/// </summary>
public class TyphoonTrackApi : QApiContractBase<TyphoonTrackRequest, TyphoonTrackResponse>
{
    public override HttpMethod Method => HttpMethod.Get;

    public override string Url => ApiConstants.Weather.TyphoonTrack;
    public async override Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        return (await base.GenerateRequestMessageAsync(option)).AddQuery($"&stormid={Request.TyphoonId}");
    }
}
public class TyphoonTrackRequest:RequestBase
{
    public string TyphoonId { get; set; }
}
public class TyphoonTrackResponse
{
    [JsonPropertyName("isActive")]
    public string IsActive { get;set; }

    [JsonPropertyName("now")]
    public TyphoonTrackItem Now { get;set; }

    [JsonPropertyName("track")]
    public List<TyphoonTrackItem> Tracks { get; set; }
}
public class TyphoonTrackItem
{
    [JsonPropertyName("pubTime")]
    private string _pubTime 
    {
        set => Time = value;
    }

    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("lat")]
    public string Lat { get; set; }

    [JsonPropertyName("lon")]
    public string Lon { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("pressure")]
    public string Pressure { get; set; }

    [JsonPropertyName("windSpeed")]
    public string WindSpeed { get; set; }

    [JsonPropertyName("moveSpeed")]
    public string MoveSpeed { get; set; }

    [JsonPropertyName("moveDir")]
    public string MoveDir { get; set; }

    [JsonPropertyName("move360")]
    public string Move360 { get; set; }

    [JsonPropertyName("windRadius30")]
    public WindRadiusItem WindRadius7 { get; set; }

    [JsonPropertyName("windRadius50")]
    public WindRadiusItem WindRadius10 { get; set; }

    [JsonPropertyName("windRadius64")]
    public WindRadiusItem WindRadius12 { get; set; }


    public class WindRadiusItem
    {
        [JsonPropertyName("neRadius")]
        public string NeRadius { get; set; }

        [JsonPropertyName("seRadius")]
        public string SeRadius { get; set; }

        [JsonPropertyName("swRadius")]
        public string SwRadius { get; set; }

        [JsonPropertyName("nwRadius")]
        public string NwRadius { get; set; }
    }
}
