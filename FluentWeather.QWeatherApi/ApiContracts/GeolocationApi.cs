using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using QWeatherApi.Bases;

namespace QWeatherApi.ApiContracts
{
    public class GeolocationApi<TResponse>: QApiContractBase<IQGeolocationRequest,TResponse> where TResponse : QWeatherResponseBase
    {

        public override HttpMethod Method => HttpMethod.Get;

        public string Url => "https://geoapi.qweather.com";

        public override string Path => ApiConstants.Geolocation.CityLookup;

        public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
        {
            var result = await base.GenerateRequestMessageAsync(option);
            result.RequestUri = new Uri(result.RequestUri.ToString().Replace("/api.qweather.com", "/geoapi.qweather.com").Replace("/devapi.qweather.com", "/geoapi.qweather.com"));
            return result;
        }
        protected override NameValueCollection GenerateQuery(ApiHandlerOption option)
        {
            var result = base.GenerateQuery(option);
            if (Request is QGeolocationRequestByLocation byLocation)
            {
                result.Add("location", $"{byLocation.Lat},{byLocation.Lon}");
            }
            else if (Request is QGeolocationRequestByName byName)
            {
                result.Add("location", $"{byName.Name}");
            }
            return result;
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
            public double Lat { get; set; }

            [JsonPropertyName("lon")]
            public double Lon { get; set; }
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
            public int Rank { get; set; }

            [JsonPropertyName("fxLink")]
            public string FxLink { get; set; }
        }
    }

}