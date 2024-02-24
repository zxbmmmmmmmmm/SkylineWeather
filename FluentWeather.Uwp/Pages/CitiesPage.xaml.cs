using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Uwp.Themes;
using FluentWeather.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using FluentWeather.Abstraction.Models;
using Newtonsoft.Json.Linq;
using FluentWeather.Uwp.Shared;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板
namespace FluentWeather.Uwp.Pages;
public sealed partial class CitiesPage : Page
{
    public CitiesPageViewModel ViewModel { get; set; } = new(); 
    public CitiesPage()
    {
        this.InitializeComponent();
        this.DataContext = this;
        this.NavigationCacheMode = NavigationCacheMode.Required;
        CurrentCityView.SelectionChanged += CurrentCityView_SelectionChanged;
        CitiesView.SelectionChanged += CitiesView_SelectionChanged;
        if (!App.ActiveArguments.Contains("City_"))
        {
            SetSelectedLocation(Common.Settings.DefaultGeolocation?.Name);
            return;
        }
        SetSelectedLocation(App.ActiveArguments.Replace("City_", ""));
    }

    public void SetSelectedLocation(string name)
    {
        if (Common.Settings.DefaultGeolocation?.Name is null)
        {
            CurrentCityView.SelectedIndex = 0;
            return;
        }
        if (ViewModel.CurrentCity is null || name == ViewModel.CurrentCity.Name)
        {
            CurrentCityView.SelectedIndex = 0;
            return;
        }
        var location = ViewModel.Cities.FirstOrDefault(p => p.Name == name);
        if (location is null) return;
        var index = ViewModel.Cities.IndexOf(location);
        CitiesView.SelectedIndex = index;
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        ((Frame)Parent)?.Navigate(typeof(SettingsPage),null,Theme.GetSplitPaneNavigationTransition());
    }

    private void CitiesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CitiesView.SelectedIndex == -1) return;
        CurrentCityView.SelectedIndex = -1;
        MainPageViewModel.Instance.CurrentGeolocation = CitiesPageViewModel.Instance.Cities[CitiesView.SelectedIndex];
        if (MainPageViewModel.Instance.CurrentGeolocation is null)
        {
            CitiesPageViewModel.Instance.GetCurrentCity();
        }
    }

    private void CurrentCityView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CurrentCityView.SelectedIndex != 0) return;
        MainPageViewModel.Instance.CurrentGeolocation = CitiesPageViewModel.Instance.CurrentCity;
        CitiesView.SelectedIndex = -1;
        if (MainPageViewModel.Instance.CurrentGeolocation is null)
        {
            CitiesPageViewModel.Instance.GetCurrentCity();
        }
    }
}