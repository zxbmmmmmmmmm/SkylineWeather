using FluentWeather.QWeatherApi.Bases;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentWeather.QWeatherApi.ApiContracts
{
    public sealed class AirConditionApi : QApiContractBase<AirConditionResponse>
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Path => ApiConstants.Weather.AirCondition;
    }
    public sealed class AirConditionResponse:QWeatherResponseBase
    {
        [JsonPropertyName("now")]
        public AirConditionItem AirConditionNow { get; set; }
        public class AirConditionItem
        {
            [JsonPropertyName("pubTime")]
            public DateTime PubTime { get; set; }

            [JsonPropertyName("aqi")]
            public int Aqi { get; set; }

            [JsonPropertyName("level")]
            public int Level { get; set; }

            [JsonPropertyName("category")]
            public string Category { get; set; }

            [JsonPropertyName("primary")]
            public string Primary { get; set; }

            [JsonPropertyName("pm10")]
            public double Pm10 { get; set; }

            [JsonPropertyName("pm2p5")]
            public double Pm2p5 { get; set; }

            [JsonPropertyName("no2")]
            public double No2 { get; set; }

            [JsonPropertyName("so2")]
            public double So2 { get; set; }

            [JsonPropertyName("co")]
            public double Co { get; set; }

            [JsonPropertyName("o3")]
            public double O3 { get; set; }
        }
    }
}
