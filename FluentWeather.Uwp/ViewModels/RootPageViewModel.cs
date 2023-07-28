using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace FluentWeather.Uwp.ViewModels;

public partial class RootPageViewModel:ObservableObject
{
    [ObservableProperty]
    private bool isPaneOpen = false;

    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    

}