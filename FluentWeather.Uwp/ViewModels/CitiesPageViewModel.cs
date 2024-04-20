using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Controls.Dialogs;
using FluentWeather.Uwp.Helpers;
using Microsoft.AppCenter.Ingestion.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Windows.UI.Xaml;
using FluentWeather.Uwp.Shared;
using Microsoft.AppCenter.Analytics;
using FluentWeather.Uwp.Helpers.Analytics;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class CitiesPageViewModel:ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<GeolocationBase> _cities;
    [ObservableProperty]
    public GeolocationBase _currentCity;
    [ObservableProperty]
    private string _query;
    [ObservableProperty]
    private List<GeolocationBase> _suggestedCities;
    public static CitiesPageViewModel Instance { get; private set; }
    public CitiesPageViewModel()
    {
        Cities = Common.Settings.SavedCities;
        Cities.CollectionChanged += async (_, _) =>
        {
            Common.Settings.SavedCities = Cities;
            await JumpListHelper.SetJumpList(CurrentCity,Cities);
        };
        Instance = this;
    }

    [RelayCommand]
    public async Task GetCities(string name)
    {
        if (name is "" or null) return;
        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();

        try
        {
            SuggestedCities = await service.GetCitiesGeolocationByName(name);
        }
        catch
        {
            await AddCustomLocation();
            return;
        }
        if(SuggestedCities.Count == 0)
        {
            await AddCustomLocation();
        }
    }
    [RelayCommand]
    public async Task AddCustomLocation()
    {
        var dialog = new LocationDialog(LocationDialogOptions.HideSearchLocation);
        await DialogManager.OpenDialogAsync(dialog);
        if (dialog.Result != null)
        {
            Cities.Add(dialog.Result);
        }
    }

    [RelayCommand]
    public void SaveCity(GeolocationBase city)
    {
        Cities.Add(city);
        Query = city.Name;
        Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackCitySaved(city.Name);
    }
    [RelayCommand]
    public void DeleteCity(GeolocationBase item)
    {
        Cities.Remove(item);
    }

    public async void GetCurrentCity()
    {
        var location = await LocationHelper.GetGeolocation();
        if (Common.Settings.DefaultGeolocation?.Name is null)
            Common.Settings.DefaultGeolocation = location;
        Common.Settings.Latitude = location.Location.Latitude;
        Common.Settings.Longitude = location.Location.Longitude;
        CitiesPageViewModel.Instance.CurrentCity = location;
        MainPageViewModel.Instance.CurrentGeolocation = location;
        await JumpListHelper.SetJumpList(Common.Settings.DefaultGeolocation, Common.Settings.SavedCities);
    }


}