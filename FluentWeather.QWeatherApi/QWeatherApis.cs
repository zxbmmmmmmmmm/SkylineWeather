using QWeatherApi.ApiContracts;

namespace QWeatherApi;

public static class QWeatherApis
{
    public static WeatherDailyApi WeatherDailyApi => new ();
    public static WeatherHourlyApi WeatherHourlyApi => new ();
    public static WeatherNowApi WeatherNowApi => new ();
    public static WeatherWarningApi WeatherWarningApi => new ();
    public static WeatherIndicesApi WeatherIndicesApi => new();
    public static PrecipitationApi PrecipitationApi => new();
    public static AirConditionApi AirConditionApi => new();
    public static TyphoonListApi TyphoonListApi => new();
    public static TyphoonTrackApi TyphoonTrackApi => new();
    public static TyphoonForecastApi TyphoonForecastApi => new();


}