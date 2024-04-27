using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using MetroLog;
using MetroLog.Targets;
using System.Collections.ObjectModel;
using Windows.Foundation.Metadata;
using System.Globalization;
using Windows.ApplicationModel;

namespace FluentWeather.Uwp.Shared;
#nullable enable
public static class Common
{
    static Common()
    {
#if DEBUG
        LogManagerFactory.DefaultConfiguration.AddTarget(MetroLog.LogLevel.Trace, MetroLog.LogLevel.Fatal, new StreamingFileTarget());
#else
	    LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Error, LogLevel.Fatal, new FileStreamingTarget());
#endif
        LogManager = LogManagerFactory.CreateLogManager();
    }
    public static Settings Settings = new();
    public static readonly ILogManager LogManager;
    public static string AppVersion = string.Format("{0}.{1}.{2}.{3}",
        Package.Current.Id.Version.Major,
        Package.Current.Id.Version.Minor,
        Package.Current.Id.Version.Build,
        Package.Current.Id.Version.Revision);
}
public sealed class Settings : INotifyPropertyChanged
{
    public string IgnoreWarningWords
    {
        get => GetSettings(nameof(IgnoreWarningWords), "");
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IgnoreWarningWords)] = value;
            OnPropertyChanged();
        }
    }
    public bool IsWarningNotificationEnabled
    {
        get => GetSettings(nameof(IsWarningNotificationEnabled), false);
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
    public bool IsTomorrowNotificationEnabled
    {
        get => GetSettings(nameof(IsTomorrowNotificationEnabled), false);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IsTomorrowNotificationEnabled)] = value;
            OnPropertyChanged();
        }
    }
    public ElementTheme ApplicationTheme
    {
        get => GetSettings(nameof(ApplicationTheme), ElementTheme.Dark);
        set
        {
            ThemeHelper.SetRequestTheme(value);
            ApplicationData.Current.LocalSettings.Values[nameof(ApplicationTheme)] = value.ToString();
            OnPropertyChanged();
        }
    }
    public bool IsDailyNotificationTileEnabled
    {
        get => GetSettings(nameof(IsDailyNotificationTileEnabled), Environment.OSVersion.Version.Build < 21996);//Win10下默认开启
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IsDailyNotificationTileEnabled)] = value;
            OnPropertyChanged();
        }
    }
    public bool UpdateLocationOnStartup
    {
        get => GetSettings(nameof(UpdateLocationOnStartup), false);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(UpdateLocationOnStartup)] = value;
            OnPropertyChanged();
        }
    }
    public int BackgroundBlurAmount
    {
        get => GetSettings(nameof(BackgroundBlurAmount), 2);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(BackgroundBlurAmount)] = value;
            OnPropertyChanged();
        }
    }
    public int BackgroundTransparency
    {
        get => GetSettings(nameof(BackgroundTransparency), 50);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(BackgroundTransparency)] = value;
            OnPropertyChanged();
        }
    }

    public int SplitViewOpenPaneLength
    {
        get => GetSettings(nameof(SplitViewOpenPaneLength), 296);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(SplitViewOpenPaneLength)] = value;
            OnPropertyChanged();
        }
    }
    public bool AutoCheckUpdates
    {
        get => GetSettings(nameof(AutoCheckUpdates), true);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(AutoCheckUpdates)] = value;
            OnPropertyChanged();
        }
    }
    public bool IsAcrylicEnabled
    {
        get => GetSettings(nameof(IsAcrylicEnabled), false);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(IsAcrylicEnabled)] = value;
            OnPropertyChanged();
        }
    }
    public double Latitude
    {
        get => GetSettings(nameof(Latitude), -1.0);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(Latitude)] = value;
            OnPropertyChanged();
        }
    }
    public double Longitude
    {
        get => GetSettings(nameof(Longitude), -1.0);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(Longitude)] = value;
            OnPropertyChanged();
        }
    }
    public GeolocationBase? DefaultGeolocation
    {
        get => GetSettingsWithClass<GeolocationBase?>(nameof(DefaultGeolocation), null);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(DefaultGeolocation)] = JsonSerializer.Serialize(value);
            OnPropertyChanged();
        }
    }
    public ObservableCollection<GeolocationBase> SavedCities
    {
        get => GetSettingsWithClass("Cities", new ObservableCollection<GeolocationBase>());
        set
        {
            ApplicationData.Current.LocalSettings.Values["Cities"] = JsonSerializer.Serialize(value);
            OnPropertyChanged();
        }
    }
    
    public string QGeolocationToken
    {
        get => GetSettings("qgeoapi." + "Token", Constants.QGeolocationToken);
        set
        {
            ApplicationData.Current.LocalSettings.Values["qgeoapi." + "Token"] = value;
            OnPropertyChanged();
        }
    }
    public string QWeatherToken
    {
        get => GetSettings("qweather." + "Token", Constants.QWeatherToken);
        set
        {
            ApplicationData.Current.LocalSettings.Values["qweather." + "Token"] = value;
            OnPropertyChanged();
        }
    }

    public string QWeatherPublicId
    {
        get => GetSettings("qweather." + "PublicId", Constants.QWeatherPublicId);
        set
        {
            ApplicationData.Current.LocalSettings.Values["qweather." + "PublicId"] = value;
            OnPropertyChanged();
        }
    }

    public string QWeatherDomain
    {
        get => GetSettings("qweather." + "Domain", Constants.QWeatherDomain??"devapi.qweather.com");
        set
        {
            ApplicationData.Current.LocalSettings.Values["qweather." + "Domain"] = value;
            OnPropertyChanged();
        }
    }

    public string Language
    {
        get => GetSettings(nameof(Language), CultureInfo.CurrentCulture.Name);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(Language)] = value;
            OnPropertyChanged();
        }
    }

    public Dictionary<string, DateTime> PushedWarnings
    {
        get => GetSettingsWithClass(nameof(PushedWarnings), new Dictionary<string, DateTime>());
        set => ApplicationData.Current.LocalSettings.Values[nameof(PushedWarnings)] = JsonSerializer.Serialize(value);
    }
    public int LastPushedTime
    {
        get => GetSettings(nameof(LastPushedTime), DateTime.Now.DayOfYear);
        set => ApplicationData.Current.LocalSettings.Values[nameof(LastPushedTime)] = value;
    }
    public int LastPushedTimeTomorrow
    {
        get => GetSettings(nameof(LastPushedTimeTomorrow), DateTime.Now.DayOfYear);
        set => ApplicationData.Current.LocalSettings.Values[nameof(LastPushedTimeTomorrow)] = value;
    }
    public AppTheme Theme
    {
        get => GetSettings(nameof(Theme), (Environment.OSVersion.Version.Build < 21996) ? AppTheme.Fluent2017 : AppTheme.Fluent);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(Theme)] = value.ToString();
            OnPropertyChanged();
        }
    }
    //public List<KeyValuePair<string, string>> DataProviderConfig
    //{
    //    get => GetSettingsWithClass<List<KeyValuePair<string, string>>>(nameof(DataProviderConfig), DataProviderHelper.QWeatherConfig);
    //    set => ApplicationData.Current.LocalSettings.Values[nameof(DataProviderConfig)] = JsonSerializer.Serialize(value);
    //}
    public ProviderConfig ProviderConfig
    {
        get => GetSettings(nameof(ProviderConfig), ProviderConfig.QWeather);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(ProviderConfig)] = value.ToString();
            OnPropertyChanged();
        }
    }

    public bool EnableCustomPage
    {
        get => GetSettings(nameof(EnableCustomPage), false);
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(EnableCustomPage)] = value;
            OnPropertyChanged();
        }
    }

    public bool OOBECompleted
    {
        get => GetSettings(nameof(OOBECompleted), (Common.Settings.QWeatherToken != null && Common.Settings.QWeatherToken != "" ));
        set
        {
            ApplicationData.Current.LocalSettings.Values[nameof(OOBECompleted)] = value;
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
                ApplicationData.Current.LocalSettings.Values[propertyName] is not null &&
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
                ApplicationData.Current.LocalSettings.Values[propertyName] is not null &&
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
public enum ProviderConfig
{
    QWeather,
    OpenMeteo
}
public enum AppTheme
{
    Fluent,
    Fluent2017,
    Classic
}