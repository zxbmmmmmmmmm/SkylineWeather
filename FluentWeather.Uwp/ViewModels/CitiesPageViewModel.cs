using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace FluentWeather.Uwp.ViewModels;

public partial class CitiesPageViewModel:ObservableObject
{
    public CitiesPageViewModel()
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        Cities = settingsHelper.ReadLocalSetting(AppSettings.Cities.ToString(),new List<GeolocationBase>());
        PropertyChanged += OnPropertyChanged;
        CurrentCity = new GeolocationBase { Name = "当前" };
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

    [ObservableProperty]
    public List<GeolocationBase> cities;

    [ObservableProperty]
    public GeolocationBase currentCity;
}