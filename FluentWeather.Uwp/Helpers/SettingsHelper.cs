using FluentWeather.Abstraction.Interfaces.Helpers;
using System.Collections;
using System.Text.Json;
using Windows.Storage;

namespace FluentWeather.Uwp.Helpers;

public sealed class SettingsHelper : ISettingsHelper
{
    public T ReadLocalSetting<T>(string settingName, T defaultValue)
    {
        var settingContainer = ApplicationData.Current.LocalSettings;

        if (IsSettingKeyExist(settingName))
        {
            if (defaultValue is Enum)
            {
                var tempValue = settingContainer.Values[settingName].ToString();
                Enum.TryParse(typeof(T), tempValue, out var result);
                return (T)result;
            }
            if (defaultValue is IList)
            {
                var tempValue = settingContainer.Values[settingName].ToString();
                return JsonSerializer.Deserialize<T>(tempValue);
            }
            return (T)settingContainer.Values[settingName];
        }
        WriteLocalSetting(settingName, defaultValue);
        return defaultValue;
    }

    public void WriteLocalSetting<T>(string settingName, T value)
    {
        var settingContainer = ApplicationData.Current.LocalSettings;

        if (value is Enum)
        {
            settingContainer.Values[settingName] = value.ToString();
        }
        else if (value is IList)
        {
            settingContainer.Values[settingName] = JsonSerializer.Serialize(value);
        }
        else
        {
            settingContainer.Values[settingName] = value;
        }
    }

    public void DeleteLocalSetting(string settingName)
    {
        var settingContainer = ApplicationData.Current.LocalSettings;

        if (IsSettingKeyExist(settingName))
        {
            settingContainer.Values.Remove(settingName);
        }
    }

    /// <inheritdoc/>
    public bool IsSettingKeyExist(string settingName)
        => ApplicationData.Current.LocalSettings.Values.ContainsKey(settingName);
}