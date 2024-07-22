using FluentWeather.Uwp.Pages;
using FluentWeather.Uwp.Shared;
using FluentWeather.Uwp.Themes;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FluentWeather.Uwp.Helpers;
using Windows.System;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls.Settings
{
    public sealed partial class DeveloperSettingSection : UserControl
    {
        public DeveloperSettingSection()
        {
            this.InitializeComponent();
        }

        private void OOBECard_Click(object sender, RoutedEventArgs e)
        {
            Common.Settings.OOBECompleted = false;
            var rootFrame = Window.Current.Content as Frame;
            rootFrame!.Navigate(typeof(WelcomePage), Theme.GetNavigationTransition());
        }

        private async void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("MetroLog");
            await Launcher.LaunchFolderAsync(folder);
        }
    }
}
