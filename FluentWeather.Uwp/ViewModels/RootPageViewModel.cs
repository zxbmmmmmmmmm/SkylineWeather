using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class RootPageViewModel:ObservableObject
{
    [ObservableProperty]
    public partial bool IsPaneOpen { get; set; }

    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

}