using FluentWeather.QWeatherApi.Bases;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherApi.ApiContracts;

public sealed class WeatherHourlyApi: QApiContractBase<WeatherHourlyResponse>
{
    public override HttpMethod Method => HttpMethod.Get;
    public override string Path => ApiConstants.Weather.HourlyForecast24H;
    public override async Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option)
    {
        var res = await base.GenerateRequestMessageAsync(option);
        if (option.Domain is "api.qweather.com")
        {
            var str = res.RequestUri.ToString();
            res.RequestUri = new System.Uri(str.Replace(Path, ApiConstants.Weather.HourlyForecast168H));
        }
        return res;
    }
}
public sealed class WeatherHourlyResponse : QWeatherResponseBase
{
    [JsonPropertyName("hourly")] public List<HourlyForecastItem> HourlyForecasts { get; set; }
    public sealed class HourlyForecastItem
    {
        [JsonPropertyName("fxTime")]
        public DateTime FxTime { get; set; }

        [JsonPropertyName("temp")]
        public int Temp { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("wind360")]
        public int Wind360 { get; set; }

        [JsonPropertyName("windDir")]
        public string WindDir { get; set; }

        [JsonPropertyName("windScale")]
        public string WindScale { get; set; }

        [JsonPropertyName("windSpeed")]
        public int WindSpeed { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("pop")]
        public int Pop { get; set; }

        [JsonPropertyName("precip")]
        public double Precipitation { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("cloud")]
        public int? Cloud { get; set; }

        [JsonPropertyName("dew")]
        public int? Dew { get; set; }
    }

}