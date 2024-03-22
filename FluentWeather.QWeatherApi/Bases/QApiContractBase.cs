using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using FluentWeather.QWeatherApi.Helpers;

namespace FluentWeather.QWeatherApi.Bases;

public abstract class QApiContractBase<TResponse>:QApiContractBase<QWeatherRequest,TResponse> where TResponse : QWeatherResponseBase
{
    public async override Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        return (await base.GenerateRequestMessageAsync(option)).AddQuery($"&location={Request.Lon},{Request.Lat}");
    }
}

public abstract class QApiContractBase<TResquest,TResponse> : ApiContractBase<TResquest, TResponse> where TResponse : QWeatherResponseBase
{
    public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var uri = "https://" + option.Domain + Path + $"?key={option.Token}";
        if (option.Language is not null)
            uri += $"&lang={option.Language}";
        var requestMessage = new HttpRequestMessage(Method, uri);

        var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
        foreach (var keyValuePair in Cookies)
        {
            cookies[keyValuePair.Key] = keyValuePair.Value;
        }
        if (cookies.Count > 0)
            requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));
        return Task.FromResult(requestMessage);
    }
    public override async Task<TResponse> ProcessResponseAsync(HttpResponseMessage response, ApiHandlerOption option)
    {
        var buffer = await response.Content.ReadAsByteArrayAsync();
        if (buffer is null || buffer.Length == 0) throw new DecoderFallbackException("返回体预读取错误");
        var str = Encoding.UTF8.GetString(buffer);
        var ret = JsonSerializer.Deserialize<TResponse>(str, option.JsonSerializerOptions);
        if (ret is null) throw new JsonException("返回 JSON 解析为空");
        return ret;
    }

}
public class QWeatherRequest : RequestBase
{
    public QWeatherRequest(double lon, double lat)
    {
        Lon = lon;
        Lat = lat;
    }
    public double Lon { get; set; }
    public double Lat { get; set; }
}
