using FluentWeather.Uwp.Themes;
using FluentWeather.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.Controls.Dialogs;

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

    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        CurrentCityView.SelectionChanged += CurrentCityView_SelectionChanged;
        CitiesView.SelectionChanged += CitiesView_SelectionChanged;
        if (App.ActiveArguments is null)
        {
            SetSelectedLocation(Common.Settings.DefaultGeolocation?.Location.GetHashCode().ToString());
            return;
        }
        SetSelectedLocation(App.ActiveArguments.Replace("City_", ""));

        if (MainPageViewModel.Instance.CurrentGeolocation is null)
        {
            CitiesPageViewModel.Instance.GetCurrentCity();
        }
    }

    public void SetSelectedLocation(string hash)
    {
        if (Common.Settings.DefaultGeolocation?.Name is null)
        {
            CurrentCityView.SelectedIndex = 0;
            return;
        }
        //if (ViewModel.CurrentCity is null || hash == ViewModel.CurrentCity.Location.GetHashCode().ToString())
        //{
        //    CurrentCityView.SelectedIndex = 0;
        //    return;
        //}
        var location = ViewModel.Cities.FirstOrDefault(p => p.Location.GetHashCode().ToString() == hash);
        if (location is null) return;
        var index = ViewModel.Cities.IndexOf(location);
        CitiesView.SelectedIndex = index;
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        ((Frame)Parent)?.Navigate(typeof(SettingsPage),null,Theme.GetSplitPaneNavigationTransition());
    }

    private async void CitiesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CitiesView.SelectedIndex == -1) return;
        CurrentCityView.SelectedIndex = -1;
        MainPageViewModel.Instance.CurrentGeolocation = CitiesPageViewModel.Instance.Cities[CitiesView.SelectedIndex];
        if (MainPageViewModel.Instance.CurrentGeolocation is null)
        {
            await CitiesPageViewModel.Instance.GetCurrentCity();
        }
    }

    private async void CurrentCityView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CurrentCityView.SelectedIndex != 0) return;
        MainPageViewModel.Instance.CurrentGeolocation = CitiesPageViewModel.Instance.CurrentCity;
        CitiesView.SelectedIndex = -1;
        if (MainPageViewModel.Instance.CurrentGeolocation is null)
        {
            await CitiesPageViewModel.Instance.GetCurrentCity();
        }
    }

    private async void GetHistoricalWeatherItem_Click(object sender, RoutedEventArgs e)
    {
        await DialogManager.OpenDialogAsync(new HistoricalWeatherSetupDialog(Common.Settings.DefaultGeolocation));
    }
}