using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Uwp.QWeatherProvider;
using FluentWeather.Uwp.QWeatherProvider.Views;
using FluentWeather.Uwp.Shared;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using FluentWeather.Uwp.Pages;
using FluentWeather.Uwp.Themes;
using static QWeatherApi.ApiConstants;

namespace FluentWeather.Uwp.ViewModels;

public partial class WelcomePageViewModel:ObservableObject
{
    [RelayCommand]
    public async Task SetProvider(string providerId)
    {
        if(providerId is "qweather")
        {
            await DialogManager.OpenDialogAsync(new SetTokenDialog());           
        }
        if (providerId is "open-meteo")
        {
            var rootFrame = Window.Current.Content as Frame;
            Common.Settings.OOBECompleted = true;
            rootFrame!.Navigate(typeof(RootPage), Theme.GetNavigationTransition());
        }
    }
}