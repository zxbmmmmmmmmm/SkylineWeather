using System.Collections.Generic;
using Windows.ApplicationModel;
using FluentWeather.Uwp.Shared;

namespace FluentWeather.Uwp.Helpers.Analytics;

public abstract class AppAnalyticsService
{
    public bool IsStarted { get; protected set; }

    private string _appVersion => string.Format("{0}.{1}.{2}.{3}",
        Package.Current.Id.Version.Major,
        Package.Current.Id.Version.Minor,
        Package.Current.Id.Version.Build,
        Package.Current.Id.Version.Revision);

    public virtual void Start()
    {
        IsStarted = true;
    }

    #region Events
    protected virtual void AddDefaultProperties(IDictionary<string, string> properties)
    {
        properties.Add("Version", _appVersion);
        properties.Add("Provider", Common.Settings.ProviderConfig.ToString());
    }

    public virtual void TrackEvent(string name, IDictionary<string, string> properties = null, bool addDefaultProperties = true)
    {
        if (addDefaultProperties)
        {
            properties ??= new Dictionary<string, string>();
            AddDefaultProperties(properties);
        }
    }

    public virtual void TrackEvent(string name, string propertyName, string propertyValue, bool addDefaultProperties = true)
    {
        TrackEvent(name, new Dictionary<string, string> { { propertyName, propertyValue } }, addDefaultProperties);
    }

    public virtual void TrackEvent(string name, string propertyName1, string propertyValue1, string propertyName2, string propertyValue2, bool addDefaultProperties = true)
    {
        TrackEvent(name, new Dictionary<string, string> { { propertyName1, propertyValue1 }, { propertyName2, propertyValue2 } }, addDefaultProperties);
    }

    public virtual void TrackTokenChanged()
    {
        TrackEvent("TokenChanged");
    }
    public virtual void TrackWeatherDataObtained()
    {
        TrackEvent("TrackWeatherDataObtained");
    }
    public virtual void TrackTyphoonDialogOpened()
    {
        TrackEvent("TrackTyphoonDialogOpened");
    }
    public virtual void TrackProviderChanged(string providerName)
    {
        TrackEvent("TrackProviderChanged","ProviderName",providerName,false);
    }
    public virtual void TrackThemeChanged(string themeName)
    {
        TrackEvent("ThemeChanged","ThemeName",themeName);
    }
    public virtual void TrackMainPageChanged(string fileName)
    {
        TrackEvent("MainPageChanged", "FileName", fileName);
    }
    public virtual void TrackDailyViewEntered()
    {
        TrackEvent("TrackDailyViewEntered");
    }
    public virtual void TrackUpdateManualChecked()
    {
        TrackEvent("TrackUpdateManualChecked");
    }
    public virtual void TrackAboutOpened()
    {
        TrackEvent("TrackAboutOpened");
    }
    public virtual void TrackUpdateViewed(string updateVersion)
    {
        TrackEvent("TrackUpdateViewed","UpdateVersion",updateVersion);
    }
    public virtual void TrackCitySaved(string cityName)
    {
        TrackEvent("TrackCitySaved", "CityName",cityName);
    }
    public virtual void TrackDefaultLocationChanged(string cityName)
    {
        TrackEvent("TrackAboutOpened", "CityName", cityName);
    }
    #endregion
}