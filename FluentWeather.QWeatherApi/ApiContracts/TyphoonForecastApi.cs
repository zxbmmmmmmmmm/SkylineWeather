using FluentWeather.QWeatherApi.Bases;
using FluentWeather.QWeatherApi.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherApi.ApiContracts
{
    public sealed class TyphoonForecastApi : QApiContractBase<TyphoonForecastRequest, TyphoonForecastResponse>
    {
        public override HttpMethod Method => HttpMethod.Get;

        public override string Path => ApiConstants.Weather.TyphoonForecast;
        protected override NameValueCollection GenerateQuery(ApiHandlerOption option)
        {
            var result = base.GenerateQuery(option);
            result.Add("stormid", Request.TyphoonId);
            return result;
        }
    }
    public sealed class TyphoonForecastRequest : RequestBase
    {
        public string TyphoonId { get; set; }
    }
    public sealed class TyphoonForecastResponse : QWeatherResponseBase
    {
        [JsonPropertyName("forecast")]
        public List<TyphoonForecastItem> Forecasts { get; set; }
        public class TyphoonForecastItem
        {
            [JsonPropertyName("fxTime")]
            public string FxTime { get; set; }

            [JsonPropertyName("lat")]
            public string Lat { get; set; }

            [JsonPropertyName("lon")]
            public string Lon { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("pressure")]
            public string Pressure { get; set; }

            [JsonPropertyName("windSpeed")]
            public string WindSpeed { get; set; }

            [JsonPropertyName("moveSpeed")]
            public string MoveSpeed { get; set; }

            [JsonPropertyName("moveDir")]
            public string MoveDir { get; set; }

            [JsonPropertyName("move360")]
            public string Move360 { get; set; }
        }
    }
}
