using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using FluentWeather.QWeatherApi.Bases;

namespace FluentWeather.QWeatherApi.ApiContracts;

public class WeatherWarningApi: QApiContractBase<WeatherWarningResponse>
{
    public override HttpMethod Method => HttpMethod.Get;
    public override string Path => ApiConstants.Weather.WarningNow;
}
public class WeatherWarningResponse : QWeatherResponseBase
{
    [JsonPropertyName("warning")]
    public List<WeatherWarningItem> Warnings { get; set; }
    public class WeatherWarningItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("sender")]
        public string Sender { get; set; }

        [JsonPropertyName("pubTime")]
        public string PubTime { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("severity")]
        public string Severity { get; set; }

        [JsonPropertyName("severityColor")]
        public string SeverityColor { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("typeName")]
        public string TypeName { get; set; }

        [JsonPropertyName("urgency")]
        public string Urgency { get; set; }

        [JsonPropertyName("certainty")]
        public string Certainty { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("related")]
        public string Related { get; set; }
    }
}