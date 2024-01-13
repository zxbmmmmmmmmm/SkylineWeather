using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.DIContainer;
using System;
using Microsoft.Extensions.DependencyInjection;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using System.Collections.Generic;
using FluentWeather.Uwp.Shared;

namespace FluentWeather.Uwp.Helpers;

public static class DIFactory
{
    public static void RegisterRequiredServices()
    {
        Locator.ServiceDescriptors.AddSingleton(typeof(ISettingsHelper), typeof(SettingsHelper));
        RegisterProviders(Common.Settings.DataProviderConfig);
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
    public static void RegisterProviders(List<KeyValuePair<string, string>> dic)
    {
        foreach (var item in dic)
        {

            var name = DataProviderHelper.GetProviderInterfaceByName(item.Key);
            var provider = DataProviderHelper.GetProviderTypeById(item.Value);
            if (name is null || provider is null) continue;
            Locator.ServiceDescriptors.AddSingleton(name,provider);
        }
    }
}