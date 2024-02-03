using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FluentWeather.Uwp.Shared;
using Microsoft.AppCenter.Analytics;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

[ObservableObject]
public sealed partial class SetLocationDialog : ContentDialog
{
    public SetLocationDialog()
    {
        this.InitializeComponent();
        if(Common.Settings.DefaultGeolocation.Name is not null)
        {
            SecondaryButtonText = "取消";
        }
    }
    [ObservableProperty]
    private string _query;
    [ObservableProperty]
    private ObservableCollection<GeolocationBase> _suggestedCities = new();
    [ObservableProperty]
    private GeolocationBase _chosenGeolocation;

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Hide();
    }

    private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Common.Settings.DefaultGeolocation = ChosenGeolocation;
        Hide();
        Analytics.TrackEvent("DefaultLocationChanged", new Dictionary<string, string> { { "CityName", ChosenGeolocation.Name } });
        await JumpListHelper.SetJumpList(Common.Settings.DefaultGeolocation, Common.Settings.SavedCities);
    }

    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        SuggestedCities.Clear();
        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        (await service.GetCitiesGeolocationByName(sender.Text))?.ForEach(SuggestedCities.Add);
    }

    private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        ChosenGeolocation = (GeolocationBase)args.SelectedItem;
        sender.Text = ChosenGeolocation.Name;
    }
}