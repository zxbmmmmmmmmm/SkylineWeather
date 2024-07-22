using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class RootPageViewModel:ObservableObject
{
    [ObservableProperty]
    private bool _isPaneOpen = false;

    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

}