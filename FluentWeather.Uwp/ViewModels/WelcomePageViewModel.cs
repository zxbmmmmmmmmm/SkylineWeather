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
using Windows.ApplicationModel.Core;

namespace FluentWeather.Uwp.ViewModels;

public partial class WelcomePageViewModel:ObservableObject
{
    [RelayCommand]
    public async Task SetProvider(string providerId)
    {
        if(providerId is "qweather")
        {
            Common.Settings.ProviderConfig = ProviderConfig.QWeather;
            await DialogManager.OpenDialogAsync(new SetTokenDialog());
        }
        if (providerId is "open-meteo")
        {
            Common.Settings.OOBECompleted = true;
            Common.Settings.ProviderConfig = ProviderConfig.OpenMeteo;
            await CoreApplication.RequestRestartAsync(string.Empty);
        }
    }
}