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
using System.Net;
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
using FluentWeather.Abstraction.Models.Exceptions;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Uwp.Helpers.Analytics;
using FluentWeather.Uwp.QWeatherProvider.Views;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

[ObservableObject]
public sealed partial class SetLocationDialog : ContentDialog
{
    public SetLocationDialog()
    {
        this.InitializeComponent();
        if(Common.Settings.DefaultGeolocation?.Name is not null)
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
        Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackDefaultLocationChanged(ChosenGeolocation.Name);
        await JumpListHelper.SetJumpList(Common.Settings.DefaultGeolocation, Common.Settings.SavedCities);
    }

    private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {

    }
    [RelayCommand]
    public async Task FindCities()
    {
        SuggestedCities.Clear();
        var service = Locator.ServiceProvider.GetService<IGeolocationProvider>();
        var result = await service.GetCitiesGeolocationByName(Query);
        result?.ForEach(SuggestedCities.Add);
        //try
        //{

        //}
        //catch (HttpResponseException e)
        //{
        //    if (e.Code == HttpStatusCode.BadRequest) return;
        //    Hide();
        //}
    }

    private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        ChosenGeolocation = (GeolocationBase)args.SelectedItem;
        sender.Text = ChosenGeolocation.Name;
    }
}