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

    }
}
