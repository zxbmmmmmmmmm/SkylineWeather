using FluentWeather.QWeatherApi.Bases;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentWeather.QWeatherApi.ApiContracts
{
    public class AirConditionApi : QApiContractBase<AirConditionResponse>
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Path => ApiConstants.Weather.AirCondition;
    }
    public class AirConditionResponse:QWeatherResponseBase
    {
        [JsonPropertyName("now")]
        public AirConditionItem AirConditionNow { get; set; }
        public class AirConditionItem
        {
            [JsonPropertyName("pubTime")]
            public string PubTime { get; set; }

            [JsonPropertyName("aqi")]
            public string Aqi { get; set; }

            [JsonPropertyName("level")]
            public string Level { get; set; }

            [JsonPropertyName("category")]
            public string Category { get; set; }

            [JsonPropertyName("primary")]
            public string Primary { get; set; }

            [JsonPropertyName("pm10")]
            public string Pm10 { get; set; }

            [JsonPropertyName("pm2p5")]
            public string Pm2p5 { get; set; }

            [JsonPropertyName("no2")]
            public string No2 { get; set; }

            [JsonPropertyName("so2")]
            public string So2 { get; set; }

            [JsonPropertyName("co")]
            public string Co { get; set; }

            [JsonPropertyName("o3")]
            public string O3 { get; set; }
        }
    }
}
