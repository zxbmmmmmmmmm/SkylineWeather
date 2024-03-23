using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using FluentWeather.QWeatherApi.Helpers;
using System.Web;
using System.Collections.Specialized;

namespace FluentWeather.QWeatherApi.Bases;

public abstract class QApiContractBase<TResponse>:QApiContractBase<QWeatherRequest,TResponse> where TResponse : QWeatherResponseBase
{
    protected override NameValueCollection GenerateQuery(ApiHandlerOption option)
    {
        var result = base.GenerateQuery(option);
        result.Add("location", $"{Request.Lon},{Request.Lat}");
        return result;
    }
}

public abstract class QApiContractBase<TResquest,TResponse> : ApiContractBase<TResquest, TResponse> where TResponse : QWeatherResponseBase
{
    public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var sb = new StringBuilder("https://");
        sb.Append(option.Domain).Append(Path).Append("?");
        sb.Append(GenerateQuery(option).Sort());

        var requestMessage = new HttpRequestMessage(Method, sb.ToString());

        var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
        foreach (var keyValuePair in Cookies)
        {
            cookies[keyValuePair.Key] = keyValuePair.Value;
        }
        if (cookies.Count > 0)
            requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));
        return Task.FromResult(requestMessage);
    }

    protected virtual NameValueCollection GenerateQuery(ApiHandlerOption option)
    {
        var queryCollection = HttpUtility.ParseQueryString(string.Empty);
        queryCollection.Add("key", option.Token);
        if (option.Language is not null)
            queryCollection.Add("lang", option.Language);
        return queryCollection;
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
