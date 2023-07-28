using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.DIContainer;
using System;
using Microsoft.Extensions.DependencyInjection;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;

namespace FluentWeather.Uwp.Helpers;

public static class DIFactory
{
    public static void RegisterRequiredServices()
    {
        Locator.ServiceDescriptors.AddSingleton(typeof(ISettingsHelper), typeof(SettingsHelper));
        //Locator.ServiceDescriptors.AddSingleton(typeof(string), "b18d888d25b4437cbae4bbf36990092e");
        QWeatherProvider.QWeatherProvider.RegisterRequiredServices();//最后注册天气服务
        QGeoProvider.QGeoProvider.RegisterRequiredServices();
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
}