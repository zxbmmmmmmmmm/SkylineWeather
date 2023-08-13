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

namespace FluentWeather.Uwp.ViewModels;

public partial class CitiesPageViewModel:ObservableObject
{
    public static CitiesPageViewModel Instance { get; private set; }
    public CitiesPageViewModel()
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        Cities = settingsHelper.ReadLocalSetting(AppSettings.Cities.ToString(),new ObservableCollection<GeolocationBase>());
        PropertyChanged += OnPropertyChanged;
        Instance = this;
        GetCurrentCity();
        Cities.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Cities));
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
        if(MainPageViewModel.Instance.CurrentLocation is null)
        {
            GetCurrentCity();
        }
    }
    partial void OnCurrentViewSelectionChanged(int oldValue, int newValue)
    {
        if (newValue != 0) return;
        MainPageViewModel.Instance.CurrentLocation = CurrentCity;
        Selection = -1;
        if (MainPageViewModel.Instance.CurrentLocation is null)
        {
            GetCurrentCity();
        }
    }
    [RelayCommand]
    public void SaveCity(GeolocationBase city)
    {
        Cities.Add(city);
        Query = city.Name;
    }
    [RelayCommand]
    public void DeleteCity(GeolocationBase item)
    {
        Cities.Remove(item);
    }
    [RelayCommand]
    public async Task OpenAboutDialog()
    {
        await new AboutDialog().ShowAsync();
    }
    [RelayCommand]
    public async Task OpenTyphoonDialog()
    {
        await new TyphoonDialog().ShowAsync();
    }
    public async Task<GeolocationBase> GetGeolocation()
    {
        //尝试获取位置
        //获取失败且默认位置未设置:弹出对话框，将默认位置作为当前位置
        //获取失败但默认位置已设置：将默认位置作为当前位置
        //获取成功:设置位置，并将当前位置设置为默认位置

        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        if (Common.Settings.DefaultGeolocation.Name is null)//默认位置未设置
        {
            var (lon, lat) = await LocationHelper.GetLocation();
            if (lon is -1||lat is -1)//获取位置失败
            {
                await new SetLocationDialog().ShowAsync();
                return Common.Settings.DefaultGeolocation;
            }
            var city = await service.GetCitiesGeolocationByLocation(lon, lat);
            if (city.Count is 0)//根据经纬度获取城市失败
            {
                await new SetLocationDialog().ShowAsync();
                return Common.Settings.DefaultGeolocation;
            }
            return city.First();
        }

        if(!Common.Settings.UpdateLocationOnStartup)//不更新位置
            return Common.Settings.DefaultGeolocation;

        //默认位置已设置但需要更新位置
        var (lo, la) = await LocationHelper.GetLocation();
        if (lo is -1 || la is -1)//检查失败
        {
            return Common.Settings.DefaultGeolocation;
        }
        var c = await service.GetCitiesGeolocationByLocation(lo, la);
        return c.Count is 0 ? Common.Settings.DefaultGeolocation : c.First();//若定位失败仍然使用默认位置
    }
    public async void GetCurrentCity()
    {
        if (Common.Settings.QWeatherToken is "" || Common.Settings.QGeolocationToken is "")
            return;
        var location = await GetGeolocation();
        if (Common.Settings.DefaultGeolocation.Name is null)
            Common.Settings.DefaultGeolocation = location;
        Common.Settings.Latitude = location.Latitude;
        Common.Settings.Longitude = location.Longitude;
        CitiesPageViewModel.Instance.CurrentCity = location;
        MainPageViewModel.Instance.CurrentLocation = location;
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