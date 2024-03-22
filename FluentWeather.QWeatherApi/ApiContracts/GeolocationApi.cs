using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentWeather.QWeatherApi.Bases;

namespace FluentWeather.QWeatherApi.ApiContracts
{
    public class GeolocationApi<TResponse>: QApiContractBase<IQGeolocationRequest,TResponse> where TResponse : QWeatherResponseBase
    {

        public override HttpMethod Method => HttpMethod.Get;

        public string Url => "https://geoapi.qweather.com";

        public override string Path => ApiConstants.Geolocation.CityLookup;

        public override Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
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
            var requestMessage = new HttpRequestMessage(Method, Url + Path + query);

            var cookies = option.Cookies.ToDictionary(t => t.Key, t => t.Value);
            foreach (var keyValuePair in Cookies)
            {
                cookies[keyValuePair.Key] = keyValuePair.Value;
            }
            if (cookies.Count > 0)
                requestMessage.Headers.Add("Cookie", string.Join("; ", cookies.Select(c => $"{c.Key}={c.Value}")));
            return Task.FromResult(requestMessage);
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
    public class QGeolocationResponse:QWeatherResponseBase
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

}