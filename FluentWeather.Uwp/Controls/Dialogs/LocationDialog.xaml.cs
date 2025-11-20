using CommunityToolkit.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

[ObservableObject]
public sealed partial class LocationDialog : ContentDialog
{
    public GeolocationBase Result { get; set; }
    public LocationDialogOptions? Options { get; set; }

    public LocationDialog(LocationDialogOptions? options = null)
    {
        this.InitializeComponent();
        _timeZones = TimeZoneInfo.GetSystemTimeZones();
        Options = options;
        if ((options & LocationDialogOptions.HideSearchLocation) is LocationDialogOptions.HideSearchLocation)
        {
            SearchLocationBox.Visibility = Visibility.Collapsed;
            ShowCustomLocationButton.Visibility = Visibility.Collapsed;
            CustomLocationPanel.Visibility = Visibility.Visible;
        }
        if ((options & LocationDialogOptions.HideCustomLocation) is LocationDialogOptions.HideCustomLocation)
        {
            CustomLocationPanel.Visibility = Visibility.Collapsed;
            ShowCustomLocationButton.Visibility = Visibility.Collapsed;
        }
        if ((options & LocationDialogOptions.HideCancelButton) is not LocationDialogOptions.HideCancelButton)
        {
            SecondaryButtonText = ResourceLoader.GetForCurrentView().GetString("Close");
        }
    }

    [ObservableProperty]
    private string _query;

    [ObservableProperty]
    private ObservableCollection<GeolocationBase> _suggestedCities = new();

    [RelayCommand]
    public async Task FindCities()
    {
        SuggestedCities.Clear();
        try
        {
            var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
            var result = await service.GetCitiesGeolocationByName(Query);
            result?.ForEach(SuggestedCities.Add);
        }
        catch
        {
            ShowCustomLocationButton.Visibility = Visibility.Collapsed;
            CustomLocationPanel.Visibility = Visibility.Visible;
        }
    }

    [RelayCommand]
    private void SelectSuggestedCities(GeolocationBase location)
    {
        if (location is null) return;
        Query = location.Name;
        Name = location.Name;
        Latitude = location.Location.Latitude.ToString(CultureInfo.InvariantCulture);
        Longitude = location.Location.Longitude.ToString(CultureInfo.InvariantCulture);
        AdmDistrict = location.AdmDistrict;
        AdmDistrict2 = location.AdmDistrict2;
        Country = location.Country;
        if (location.TimeZone is not null)
        {
            try
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(location.TimeZone);
            }
            catch
            {
                TimeZone = GetTimeZoneFromLocation(location.Location.Longitude);
            }
        }
        else if (location.UtcOffset is not null)
        {
            TimeZone = TimeZones.First(p => p.BaseUtcOffset == location.UtcOffset);
        }
        else
        {
            TimeZone = GetTimeZoneFromLocation(location.Location.Longitude);
        }
        IsDaylightSavingTime = location.IsDaylightSavingTime;

        if (ShowCustomLocationButton.Visibility is Visibility.Visible)
        {
            CustomLocationPanel.Visibility = Visibility.Visible;
            ShowCustomLocationButton.Visibility = Visibility.Collapsed;
        }
    }

    private bool CanContinue
    {
        get
        {
            if (Name is null or "") return false;
            if (Latitude is null or "" || !Latitude.IsDecimal()) return false;
            if (Longitude is null or "" || !Longitude.IsDecimal()) return false;
            if (TimeZone is null) return false;
            return true;
        }
    }
    [RelayCommand]
    public void Continue()
    {
        if (double.Parse(Latitude) > 90 || double.Parse(Latitude) < -90)
        {
            Latitude = "";
            return;
        }
        Result = new GeolocationBase()
        {
            Location = new(double.Parse(Latitude), double.Parse(Longitude)),
            Country = Country,
            AdmDistrict = AdmDistrict,
            AdmDistrict2 = AdmDistrict2,
            Name = Name,
            UtcOffset = TimeZone.BaseUtcOffset,
            TimeZone = TimeZone.Id
        };
        Hide();
    }
    public TimeZoneInfo GetTimeZoneFromLocation(double longitude)
    {
        var timeZone = 0;

        var quotient = (int)(longitude / 15);
        var remainder = Math.Abs(longitude % 15);
        if (remainder <= 7.5)
        {
            timeZone = quotient;
        }
        else
        {
            timeZone = quotient + (longitude > 0 ? 1 : -1);
        }

        return TimeZones.FirstOrDefault(p => p.BaseUtcOffset == TimeSpan.FromHours(1) * timeZone);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanContinue))]
    private string _name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanContinue))]
    private string _latitude;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanContinue))]
    private string _longitude;

    [ObservableProperty]
    private string _admDistrict;

    [ObservableProperty]
    private string _admDistrict2;

    [ObservableProperty]
    private string _country;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanContinue))]
    private TimeZoneInfo _timeZone;

    [ObservableProperty]
    private IList<TimeZoneInfo> _timeZones;

    [ObservableProperty]
    private bool? _isDaylightSavingTime;

    private void ShowCustomLocationButton_Click(object sender, RoutedEventArgs e)
    {
        CustomLocationPanel.Visibility = Visibility.Visible;
        ShowCustomLocationButton.Visibility = Visibility.Collapsed;
    }

    private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
    {
        var isCancelButtonHidden = (Options & LocationDialogOptions.HideCancelButton) is LocationDialogOptions.HideCancelButton;
        if (!CanContinue && isCancelButtonHidden)
        {
            args.Cancel = true;
        }
    }
}

[Flags]
public enum LocationDialogOptions
{
    /// <summary>
    /// 隐藏搜索位置
    /// </summary>
    HideSearchLocation = 1,

    /// <summary>
    /// 隐藏自定义位置
    /// </summary>
    HideCustomLocation = 2,

    /// <summary>
    /// 隐藏关闭按钮
    /// </summary>
    HideCancelButton = 4,
}