using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;
using FluentWeather.Uwp.Controls.Dialogs;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板
namespace FluentWeather.Uwp.Controls.Settings
{
    [ObservableObject]
    public sealed partial class LocationSettingSection : UserControl
    {
        public LocationSettingSection()
        {
            this.InitializeComponent();
        }
        [RelayCommand]
        private async Task OpenLocationDialog()
        {
            var dialog = new LocationDialog();
            await DialogManager.OpenDialogAsync(dialog);
            if (dialog.Result is null) return;
            Common.Settings.DefaultGeolocation = dialog.Result;
            try
            {
                await CacheHelper.DeleteUnused();
            }
            finally
            {
                await CoreApplication.RequestRestartAsync(string.Empty);
            }
        }
    }
}
