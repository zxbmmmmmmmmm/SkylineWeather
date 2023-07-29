using FluentWeather.QWeatherApi.ApiContracts;

namespace FluentWeather.QWeatherApi;

public static class QWeatherApis
{
    public static WeatherDailyApi WeatherDailyApi => new ();
    public static WeatherHourlyApi WeatherHourlyApi => new ();
    public static WeatherNowApi WeatherNowApi => new ();
    public static WeatherWarningApi WeatherWarningApi => new ();
    public static WeatherIndicesApi WeatherIndicesApi => new();
    public static PrecipitationApi PrecipitationApi => new();


}