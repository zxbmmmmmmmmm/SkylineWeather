using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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