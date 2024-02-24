namespace FluentWeather.QWeatherApi;

public static class ApiConstants
{
    public const string ApiBase = "https://devapi.qweather.com";
    public const string Domain = "devapi.qweather.com";

    public static class Weather
    {
        public const string Now = "/v7/weather/now";
        public const string DailyForecast3D = "/v7/weather/3d";
        public const string DailyForecast7D = "/v7/weather/7d";
        public const string DailyForecast10D = "/v7/weather/10d";
        public const string DailyForecast15D = "/v7/weather/15d";
        public const string DailyForecast30D = "/v7/weather/30d";
        public const string HourlyForecast24H = "/v7/weather/24h";
        public const string HourlyForecast72H = "/v7/weather/72h";
        public const string HourlyForecast168H = "/v7/weather/168h";
        public const string WarningNow = "/v7/warning/now";
        public const string WeatherIndices1D = "/v7/indices/1d";
        public const string WeatherIndices3D = "/v7/indices/3d";
        public const string MinutelyPrecipitation = "/v7/minutely/5m";
        public const string AirCondition = "/v7/air/now";
        public const string TyphoonTrack = "/v7/tropical/storm-track";
        public const string TyphoonList = "/v7/tropical/storm-list";
        public const string TyphoonForecast = "/v7/tropical/storm-forecast";
    }

    public static class Geolocation 
    {
        public const string CityLookup = "/v2/city/lookup";
    }

}
