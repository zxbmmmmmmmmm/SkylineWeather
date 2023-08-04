using FluentWeather.QGeoProvider;
using FluentWeather.QGeoProvider.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace FluentWeather.Uwp.Helpers;
#nullable enable
internal static class Common
{
    public static Settings Settings = new();
}
internal class Settings:INotifyPropertyChanged
{
    public bool IsWarningNotificationEnabled
    {
        get => GetSettings(nameof(IsWarningNotificationEnabled), true);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IsWarningNotificationEnabled)] = value;
            OnPropertyChanged();
        }
    }
    public bool IsDailyNotificationEnabled
    {
        get => GetSettings(nameof(IsDailyNotificationEnabled), false);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IsDailyNotificationEnabled)] = value;
            OnPropertyChanged();
        }
    }
    public bool IsDailyNotificationTileEnabled
    {
        get => GetSettings(nameof(IsDailyNotificationTileEnabled), true);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IsDailyNotificationTileEnabled)] = value;
            OnPropertyChanged();
        }
    }
    public string QGeolocationToken
    {
        get => GetSettings("qgeoapi."+ "Token", "");
        set
        {
            ApplicationData.Current.LocalSettings.Values["qgeoapi." + "Token"] = value;
            OnPropertyChanged();
        }
    }
    public string QWeatherToken
    {
        get => GetSettings("qweather." + "Token", "");
        set
        {
            ApplicationData.Current.LocalSettings.Values["qweather." + "Token"] = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public async void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () => { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); });
    }
    /// <summary>
    /// 获取设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetSettings<T>(string propertyName, T defaultValue)
    {
        try
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName) &&
                ApplicationData.Current.LocalSettings.Values[propertyName] != null &&
                !string.IsNullOrEmpty(ApplicationData.Current.LocalSettings.Values[propertyName].ToString()))
            {
                if (defaultValue is bool)
                    return (T)(object)bool.Parse(ApplicationData.Current.LocalSettings.Values[propertyName]
                        .ToString());
                if (defaultValue is Enum)
                {
                    var tempValue = ApplicationData.Current.LocalSettings.Values[propertyName].ToString();
                    Enum.TryParse(typeof(T), tempValue, out var result);
                    return (T)result;
                }
                return (T)ApplicationData.Current.LocalSettings.Values[propertyName];
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[propertyName] = defaultValue;
            }
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
    /// <summary>
    /// 将设置转换为Json后保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetSettingsWithClass<T>(string propertyName, T defaultValue)//使用default value中的T
    {
        try
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(propertyName) &&
                ApplicationData.Current.LocalSettings.Values[propertyName] != null &&
                !string.IsNullOrEmpty(ApplicationData.Current.LocalSettings.Values[propertyName].ToString()))
            {
                if (typeof(T).ToString() == "System.Boolean")
                    return (T)(object)bool.Parse(ApplicationData.Current.LocalSettings.Values[propertyName]
                        .ToString());
                var str = (string)ApplicationData.Current.LocalSettings.Values[propertyName];//获取字符串
                return JsonSerializer.Deserialize<T>(str);
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[propertyName] = JsonSerializer.Serialize(defaultValue);
            }
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}