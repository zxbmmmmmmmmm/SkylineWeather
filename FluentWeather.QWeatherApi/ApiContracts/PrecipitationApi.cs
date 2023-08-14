using FluentWeather.QWeatherApi.Bases;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentWeather.QWeatherApi.ApiContracts
{
    public class PrecipitationApi : QApiContractBase<PrecipitationResponse>
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Path => ApiConstants.Weather.MinutelyPrecipitation;
    }
    public class PrecipitationResponse:QWeatherResponseBase
    {
        [JsonPropertyName("minutely")]
        public List<PrecipitationItem> MinutelyPrecipitations { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        public class PrecipitationItem
        {
            [JsonPropertyName("fxTime")]
            public string FxTime { get; set; }

            [JsonPropertyName("precip")]
            public string Precip { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
    }
}
