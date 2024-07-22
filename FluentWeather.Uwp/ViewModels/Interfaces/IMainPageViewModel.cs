using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.ViewModels.Interfaces;

public interface IMainPageViewModel
{
    /// <summary>
    /// 每日预报
    /// </summary>
    List<WeatherDailyBase> DailyForecasts { get; }

    /// <summary>
    /// 每小时预报
    /// </summary>
    List<WeatherHourlyBase> HourlyForecasts { get; }

    /// <summary>
    /// 天气指数
    /// </summary>
    List<IndicesBase> Indices { get; }

    /// <summary>
    /// 当日历史天气
    /// </summary>
    HistoricalDailyWeatherBase HistoricalWeather { get; }

    /// <summary>
    /// 当前天气
    /// </summary>
    WeatherNowBase WeatherNow { get; }

    /// <summary>
    /// 当前位置
    /// </summary>
    GeolocationBase CurrentGeolocation { get; }

    /// <summary>
    /// 刷新
    /// </summary>
    IAsyncRelayCommand RefreshCommand { get; }
}