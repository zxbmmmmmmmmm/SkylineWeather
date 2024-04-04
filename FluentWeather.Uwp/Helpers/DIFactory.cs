using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.DIContainer;
using System;
using Microsoft.Extensions.DependencyInjection;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using System.Collections.Generic;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.QWeatherProvider;
using static FluentWeather.DIContainer.Locator;
using Windows.Media.Protection.PlayReady;
using FluentWeather.Uwp.Helpers.Analytics;
using FluentWeather.BingGeolocationProvider;

namespace FluentWeather.Uwp.Helpers;

public static class DIFactory
{
    public static void RegisterRequiredServices()
    {
        ServiceDescriptors.AddSingleton(typeof(ISettingsHelper), typeof(SettingsHelper));
        ServiceDescriptors.AddSingleton(typeof(AppAnalyticsService), typeof(AppCenterAnalyticsService));

        switch (Common.Settings.ProviderConfig)
        {
            case ProviderConfig.QWeather:
                RegisterQWeather();
                break;
            case ProviderConfig.OpenMeteo:
                RegisterOpenMeteo();
                break;
            default:
                RegisterQWeather();
                break;
        }

        Locator.ServiceProvider = ServiceDescriptors.BuildServiceProvider();
    }
    public static void ReadSettings()
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        var settingsList = Locator.ServiceProvider.GetServices<ISetting>();//找出实现ISettings的类并读取设置内容
        foreach (var item in settingsList)
        {
            foreach (var setting in Enum.GetNames(item.Settings.GetType()))
            {
                settingsHelper.ReadLocalSetting(item.Id + "." + setting, new object());
            }
        }
    }
    public static void RegisterQWeather()
    {
        ServiceDescriptors.AddSingleton(typeof(ICurrentWeatherProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IDailyForecastProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IHourlyForecastProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IWeatherWarningProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IIndicesProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IPrecipitationProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IAirConditionProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(ITyphoonProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(ISetting), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IGeolocationProvider), typeof(BingGeolocationProvider.BingGeolocationProvider));
    }
    public static void RegisterOpenMeteo()
    {
        ServiceDescriptors.AddSingleton(typeof(ICurrentWeatherProvider), typeof(OpenMeteoProvider.OpenMeteoProvider));
        ServiceDescriptors.AddSingleton(typeof(IAirConditionProvider), typeof(OpenMeteoProvider.OpenMeteoProvider));
        ServiceDescriptors.AddSingleton(typeof(IDailyForecastProvider), typeof(OpenMeteoProvider.OpenMeteoProvider));
        ServiceDescriptors.AddSingleton(typeof(IHourlyForecastProvider), typeof(OpenMeteoProvider.OpenMeteoProvider));
        ServiceDescriptors.AddSingleton(typeof(IWeatherWarningProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IIndicesProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IPrecipitationProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(ITyphoonProvider), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(ISetting), typeof(QWeatherProvider.QWeatherProvider));
        ServiceDescriptors.AddSingleton(typeof(IGeolocationProvider), typeof(BingGeolocationProvider.BingGeolocationProvider));
        OpenMeteoProvider.OpenMeteoProvider.Client.ForecastParameters.Add("forecast_hours", "168");
    }
    //public static void RegisterProviders(List<KeyValuePair<string, string>> dic)
    //{
    //    foreach (var item in dic)
    //    {

    //        var name = DataProviderHelper.GetProviderInterfaceByName(item.Key);
    //        var provider = DataProviderHelper.GetProviderTypeById(item.Value);
    //        if (name is null || provider is null) continue;
    //        Locator.ServiceDescriptors.AddSingleton(name, provider);
    //    }
    //}
}