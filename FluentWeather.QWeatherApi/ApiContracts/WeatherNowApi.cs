using FluentWeather.QWeatherApi.Bases;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace FluentWeather.QWeatherApi.ApiContracts;

public class WeatherNowApi:ApiContractBase<WeatherNowResponse>
{
    public override HttpMethod Method => HttpMethod.Get;
    public override string Url => ApiConstants.Weather.Now;
}
public class WeatherNowResponse : QWeatherResponseBase
{
    [JsonPropertyName("now")]
    public WeatherNowItem WeatherNow { get; set; }
    public class WeatherNowItem
    {
        [JsonPropertyName("obsTime")]
        public string ObsTime { get; set; }

        [JsonPropertyName("temp")]
        public string Temp { get; set; }

        [JsonPropertyName("feelsLike")]
        public string FeelsLike { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("wind360")]
        public string Wind360 { get; set; }

        [JsonPropertyName("windDir")]
        public string WindDir { get; set; }

        [JsonPropertyName("windScale")]
        public string WindScale { get; set; }

        [JsonPropertyName("windSpeed")]
        public string WindSpeed { get; set; }

        [JsonPropertyName("humidity")]
        public string Humidity { get; set; }

        [JsonPropertyName("precip")]
        public string Precipitation { get; set; }

        [JsonPropertyName("pressure")]
        public string Pressure { get; set; }

        [JsonPropertyName("vis")]
        public string Vis { get; set; }

        [JsonPropertyName("cloud")]
        public string Cloud { get; set; }

        [JsonPropertyName("dew")]
        public string Dew { get; set; }
    }
}
