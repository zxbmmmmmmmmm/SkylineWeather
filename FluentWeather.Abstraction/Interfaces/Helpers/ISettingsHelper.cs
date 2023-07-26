namespace FluentWeather.Abstraction.Interfaces.Helpers;

public interface ISettingsHelper
{

    void WriteLocalSetting<T>(string settingName, T value);
    T ReadLocalSetting<T>(string settingName, T defaultValue);
    void DeleteLocalSetting(string settingName);
    bool IsSettingKeyExist(string settingName);
}