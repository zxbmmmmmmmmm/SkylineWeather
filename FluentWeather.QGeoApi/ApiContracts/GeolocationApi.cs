using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentWeather.QGeoApi.Bases;
using System;

namespace FluentWeather.QGeoApi.ApiContracts
{
    public class GeolocationApi<TResponse>
    {
        public HttpMethod Method => HttpMethod.Get;
        public string Url => "https://geoapi.qweather.com/v2/city/lookup";
        public virtual Dictionary<string, string> Cookies { get; } = new();
        public IQGeolocationRequest Request { get; set; }
        public Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
        {
            string query = string.Empty;
            if(Request is QGeolocationRequestByLocation byLocation)
            {
                query = $"?key={option.Token}&location={byLocation.Lat},{byLocation.Lon}";
            }
            else if(Request is QGeolocationRequestByName byName)
            {
                query = $"?key={option.Token}&location={byName.Name}";
            }
            if (option.Language is not null)
                query += $"&lang={option.Language}";
            var requestMessage = new HttpRequestMessage(Method, Url + query);

            var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
            foreach (var keyValuePair in Cookies)
            {
                cookies[keyValuePair.Key] = keyValuePair.Value;
            }
            if (cookies.Count > 0)
                requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));
            return Task.FromResult(requestMessage);
        }
        public async Task<TResponse> ProcessResponseAsync(HttpResponseMessage response, ApiHandlerOption option)
        {
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"请求返回 HTTP 代码: {response.StatusCode}");

            var buffer = await response.Content.ReadAsByteArrayAsync();
            if (buffer is null || buffer.Length == 0) throw new DecoderFallbackException("返回体预读取错误");
            var ret = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(buffer), option.JsonSerializerOptions);

            if (ret is null) throw new JsonException("返回 JSON 解析为空");
            return ret;
        }
    }
    /// <summary>
    /// 通过城市名称查询
    /// </summary>
    public class QGeolocationRequestByName: IQGeolocationRequest
    {
        public string Name{ get; set; }
    }
    /// <summary>
    /// 通过位置查询
    /// </summary>
    public class QGeolocationRequestByLocation: IQGeolocationRequest
    {
        public double Lon { get; set; }
        public double Lat { get; set; }

    }

    public interface IQGeolocationRequest
    {
        
    }
    public class QGeolocationResponse : QGeolocationResponseBase
    {
        [JsonPropertyName("location")]
        public List<QGeolocationItem> Locations { get; set; }
        public class QGeolocationItem
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("lat")]
            public string Lat { get; set; }

            [JsonPropertyName("lon")]
            public string Lon { get; set; }
            /// <summary>
            /// 地区/城市的上级行政区划名称(如:北京)
            /// </summary>
            [JsonPropertyName("adm2")]
            public string AdministrativeDistrict2 { get; set; }

            /// <summary>
            /// 地区/城市所属一级行政区域(如:北京市)
            /// </summary>
            [JsonPropertyName("adm1")]
            public string AdministrativeDistrict1 { get; set; }

            [JsonPropertyName("country")]
            public string Country { get; set; }

            [JsonPropertyName("tz")]
            public string TimeZone { get; set; }

            [JsonPropertyName("utcOffset")]
            public string UtcOffset { get; set; }

            [JsonPropertyName("isDst")]
            public string IsDaylightSavingTime { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("rank")]
            public string Rank { get; set; }

            [JsonPropertyName("fxLink")]
            public string FxLink { get; set; }
        }
    }
    public class ApiHandlerOption
    {
        public Dictionary<string, string> Cookies { get; } = new();
        public string Token { get; set; }
        public string Language { get; set; }

        public JsonSerializerOptions JsonSerializerOptions =
            new(JsonSerializerOptions.Default)
            {
                NumberHandling = JsonNumberHandling.WriteAsString |
                                 JsonNumberHandling.AllowReadingFromString,
                AllowTrailingCommas = true,
            };
    }
}