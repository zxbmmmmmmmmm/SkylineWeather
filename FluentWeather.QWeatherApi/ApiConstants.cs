namespace FluentWeather.QWeatherApi;

public static class ApiConstants
{
    public const string ApiBase = "https://api.qweather.com";
    public const string Domain = "api.qweather.com";

    public static class Weather
    {
        public const string Now = ApiBase + "/v7/weather/now";
        public const string DailyForecast3D = ApiBase + "/v7/weather/3d";
        public const string DailyForecast7D = ApiBase + "/v7/weather/7d";
        public const string DailyForecast10D = ApiBase + "/v7/weather/10d";
        public const string DailyForecast15D = ApiBase + "/v7/weather/15d";
        public const string HourlyForecast24H = ApiBase + "/v7/weather/24h";
        public const string HourlyForecast72H = ApiBase + "/v7/weather/72h";
        public const string HourlyForecast168H = ApiBase + "/v7/weather/168h";
        public const string WarningNow = ApiBase + "/v7/warning/now";
        public const string WeatherIndices1D = ApiBase + "/v7/indices/1d";
        public const string WeatherIndices3D = ApiBase + "/v7/indices/3d";
        public const string MinutelyPrecipitation = ApiBase + "/v7/minutely/5m";
        public const string AirCondition = ApiBase + "/v7/air/now";
        public const string TyphoonTrack = ApiBase + "/v7/tropical/storm-track";
        public const string TyphoonList = ApiBase + "/v7/tropical/storm-list";
        public const string TyphoonForecast = ApiBase + "/v7/tropical/storm-forecast";
    }
}
