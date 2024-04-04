using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FluentWeather.Uwp.Controls.Dialogs;
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
            await DialogManager.OpenDialogAsync(new SetLocationDialog());
        }
    }
}
