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
using FluentWeather.Uwp.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace FluentWeather.Uwp.ViewModels;

public partial class CitiesPageViewModel:ObservableObject
{
    public CitiesPageViewModel()
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        Cities = settingsHelper.ReadLocalSetting(AppSettings.Cities.ToString(),new ObservableCollection<GeolocationBase>());
        PropertyChanged += OnPropertyChanged;
        GetCurrentCity();
        Cities.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Cities));
    }
    public async void GetCurrentCity()
    {
        await LocationHelper.GetLocation();
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        var lon = settingsHelper.ReadLocalSetting(AppSettings.Longitude.ToString(),116.0);
        var lat = settingsHelper.ReadLocalSetting(AppSettings.Latitude.ToString(), 40.0);

        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        var city = await service.GetCitiesGeolocationByLocation(lon, lat);
        CurrentCity = city.First();
        MainPageViewModel.Instance.CurrentLocation = CurrentCity;
    }
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        var settings = Locator.ServiceProvider.GetService<ISetting>();
        //Token = settingsHelper.ReadLocalSetting(settings.Id + "." + nameof(Token), "");

        switch (e.PropertyName)
        {
            case nameof(Cities):
                settingsHelper.WriteLocalSetting(AppSettings.Cities.ToString(), Cities);
                break;
        }
    }
    [RelayCommand]
    public async Task GetCities(string name)
    {
        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        SuggestedCities = await service.GetCitiesGeolocationByName(name);
    }
    partial void OnSelectionChanged(int oldValue, int newValue)
    {
        if (newValue == -1) return;
        CurrentViewSelection = -1;
        MainPageViewModel.Instance.CurrentLocation = Cities[newValue];
        return;
    }
    partial void OnCurrentViewSelectionChanging(int oldValue, int newValue)
    {
        if (newValue != 0) return;
        MainPageViewModel.Instance.CurrentLocation = CurrentCity;
        Selection = -1;
    }
    [RelayCommand]
    public void SaveCity(GeolocationBase city)
    {
        Cities.Add(city);
        Query = city.Name;
    }

    [ObservableProperty]
    private ObservableCollection<GeolocationBase> cities;

    [ObservableProperty]
    private GeolocationBase currentCity;

    [ObservableProperty]
    private string query;
    [ObservableProperty]
    private List<GeolocationBase> suggestedCities;
    [ObservableProperty]
    private int selection = -1;
    [ObservableProperty]
    private int currentViewSelection;
}