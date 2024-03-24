using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using QWeatherApi.Bases;

namespace QWeatherApi.ApiContracts
{
    public sealed class PrecipitationApi : QApiContractBase<PrecipitationResponse>
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Path => ApiConstants.Weather.MinutelyPrecipitation;
    }
    public sealed class PrecipitationResponse:QWeatherResponseBase
    {
        [JsonPropertyName("minutely")]
        public List<PrecipitationItem> MinutelyPrecipitations { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        public class PrecipitationItem
        {
            [JsonPropertyName("fxTime")]
            public DateTime FxTime { get; set; }

            [JsonPropertyName("precip")]
            public double Precip { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
    }
}
