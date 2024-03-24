using FluentWeather.QWeatherApi.Bases;
using System.Net.Http;
using System.Text.Json.Serialization;
using System;
namespace FluentWeather.QWeatherApi.ApiContracts;

public sealed class WeatherNowApi: QApiContractBase<WeatherNowResponse>
{
    public override HttpMethod Method => HttpMethod.Get;
    public override string Path => ApiConstants.Weather.Now;
}
public sealed class WeatherNowResponse : QWeatherResponseBase
{
    [JsonPropertyName("now")]
    public WeatherNowItem WeatherNow { get; set; }
    public sealed class WeatherNowItem
    {
        [JsonPropertyName("obsTime")]
        public DateTime ObsTime { get; set; }

        [JsonPropertyName("temp")]
        public int Temp { get; set; }

        [JsonPropertyName("feelsLike")]
        public int FeelsLike { get; set; }

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

        [JsonPropertyName("precip")]
        public double Precipitation { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("vis")]
        public int Vis { get; set; }

        [JsonPropertyName("cloud")]
        public int Cloud { get; set; }

        [JsonPropertyName("dew")]
        public int Dew { get; set; }
    }
}
