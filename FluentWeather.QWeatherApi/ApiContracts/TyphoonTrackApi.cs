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
public sealed class TyphoonTrackApi : QApiContractBase<TyphoonTrackRequest, TyphoonTrackResponse>
{
    public override HttpMethod Method => HttpMethod.Get;

    public override string Path => ApiConstants.Weather.TyphoonTrack;
    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        return (await base.GenerateRequestMessageAsync(option)).AddQuery($"&stormid={Request.TyphoonId}");
    }
}
public sealed class TyphoonTrackRequest:RequestBase
{
    public string TyphoonId { get; set; }
}
public sealed class TyphoonTrackResponse : QWeatherResponseBase
{
    [JsonPropertyName("isActive")]
    public string IsActive { get;set; }

    [JsonPropertyName("now")]
    public TyphoonTrackItem Now { get;set; }

    [JsonPropertyName("track")]
    public List<TyphoonTrackItem> Tracks { get; set; }
}
public sealed class TyphoonTrackItem
{
    [JsonPropertyName("pubTime")]
    private DateTime _pubTime 
    {
        set => Time = value;
    }

    [JsonPropertyName("time")]
    public DateTime? Time { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; set; }

    [JsonPropertyName("windSpeed")]
    public int WindSpeed { get; set; }

    [JsonPropertyName("moveSpeed")]
    public int MoveSpeed { get; set; }

    [JsonPropertyName("moveDir")]
    public string MoveDir { get; set; }

    [JsonPropertyName("move360")]
    public int Move360 { get; set; }

    [JsonPropertyName("windRadius30")]
    public WindRadiusItem WindRadius7 { get; set; }

    [JsonPropertyName("windRadius50")]
    public WindRadiusItem WindRadius10 { get; set; }

    [JsonPropertyName("windRadius64")]
    public WindRadiusItem WindRadius12 { get; set; }


    public sealed class WindRadiusItem
    {
        [JsonPropertyName("neRadius")]
        public int NeRadius { get; set; }

        [JsonPropertyName("seRadius")]
        public int SeRadius { get; set; }

        [JsonPropertyName("swRadius")]
        public int SwRadius { get; set; }

        [JsonPropertyName("nwRadius")]
        public int NwRadius { get; set; }
    }
}
