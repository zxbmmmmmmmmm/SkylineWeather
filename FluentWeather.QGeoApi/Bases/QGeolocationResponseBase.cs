using System.Text.Json.Serialization;

namespace FluentWeather.QGeoApi.Bases
{
    public class QGeolocationResponseBase
    {
        [JsonPropertyName("code")]
        public string Code{ get; set; }
    }
}