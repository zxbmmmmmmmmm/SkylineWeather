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
    public virtual void TrackAnnouncementViewed(string name)
    {
        TrackEvent($"{name} Viewed");
    }
    public virtual void TrackAnnouncementClosed(string name,bool isViewed)
    {
        TrackEvent($"{name} Closed","IsViewed",isViewed.ToString());
    }
    public virtual void TrackHistoricalWeatherDataDownloaded(string location)
    {
        TrackEvent("HistoricalWeatherDataDownloaded","Geolocation",location);
    }
    public virtual void TrackTokenChanged()
    {
        TrackEvent("TokenChanged");
    }
    public virtual void TrackWeatherDataObtained()
    {
        TrackEvent("WeatherDataObtained");
    }
    public virtual void TrackTyphoonDialogOpened()
    {
        TrackEvent("TyphoonDialogOpened");
    }
    public virtual void TrackProviderChanged(string providerName)
    {
        TrackEvent("ProviderChanged","ProviderName",providerName,false);
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
        TrackEvent("DailyViewEntered");
    }
    public virtual void TrackUpdateManualChecked()
    {
        TrackEvent("UpdateManualChecked");
    }
    public virtual void TrackAboutOpened()
    {
        TrackEvent("AboutOpened");
    }
    public virtual void TrackUpdateViewed(string updateVersion)
    {
        TrackEvent("UpdateViewed","UpdateVersion",updateVersion);
    }
    public virtual void TrackCitySaved(string cityName)
    {
        TrackEvent("CitySaved", "CityName",cityName);
    }
    public virtual void TrackDefaultLocationChanged(string cityName)
    {
        TrackEvent("DefaultLocationChanged", "CityName", cityName);
    }
    public virtual void TrackDeveloperModeEnabled()
    {
        TrackEvent("DeveloperModeEnabled");
    }
    #endregion
}