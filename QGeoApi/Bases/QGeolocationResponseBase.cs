using System.Text.Json.Serialization;

namespace QGeoApi.Bases
{
    public class QGeolocationResponseBase
    {
        [JsonPropertyName("code")]
        public string Code{ get; set; }
    }
}