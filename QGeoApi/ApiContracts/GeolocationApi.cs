using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using QGeoApi.Bases;

namespace QGeoApi.ApiContracts
{
    public class GeolocationApi
    {
        public HttpMethod Method => HttpMethod.Get;
        public string Url => "https://geoapi.qweather.com/v2/city/lookup";
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
}