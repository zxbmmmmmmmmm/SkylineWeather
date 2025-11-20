using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Behaviors;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Helpers.Analytics;
using FluentWeather.Uwp.Pages;
using FluentWeather.Uwp.Shared;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板
namespace FluentWeather.Uwp.Controls.Settings
{
    [ObservableObject]
    public sealed partial class PersonalizationSettingSection : UserControl
    {
        public PersonalizationSettingSection()
        {
            this.InitializeComponent();
        }
        [RelayCommand]
        public async Task SetBackground(string type)
        {
            var imageFile = await PickFileAsync();
            if (imageFile is null) return;
            if ((await ApplicationData.Current.LocalFolder.TryGetItemAsync("Backgrounds")) is null)
            {
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("Backgrounds");
            }
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Backgrounds");
            if (await folder.TryGetItemAsync(type + ".png") is StorageFile file)
            {
                await file.DeleteAsync();
            }
            await imageFile.CopyAsync(folder, type + ".png");
            LoadLocalBackgroundBehavior.Instance?.LoadImage();
        }
        [RelayCommand]
        public async Task OpenBackgroundsFolder()
        {
            var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("Backgrounds");
            await Launcher.LaunchFolderAsync(folder);
        }
        private async Task<StorageFile> PickFileAsync()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            StorageFile file = await picker.PickSingleFileAsync();

            return file;
        }

        private void ThemeButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((RadioButton)e.AddedItems[0])?.Tag?.ToString();
            if (selected is null) return;
            Common.Settings.ApplicationTheme = Enum.Parse<ElementTheme>(selected);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeButtons.SelectedIndex = Common.Settings.ApplicationTheme switch
            {
                ElementTheme.Default => 0,
                ElementTheme.Light => 1,
                ElementTheme.Dark => 2,
                _ => ThemeButtons.SelectedIndex
            };
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            await CoreApplication.RequestRestartAsync(string.Empty);
        }

        private void ThemeStyleButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RestartInfoBar.IsOpen = true;
            Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackThemeChanged(Common.Settings.Theme.ToString());
        }

        private void ApplyBlurButton_Click(object sender, RoutedEventArgs e)
        {
            RootPage.Instance.EnterAnimation.Start();
        }

        private async void SelectPageFile_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
            };
            picker.FileTypeFilter.Add(".xaml");
            var file = await picker.PickSingleFileAsync();
            if (file is null) return;
            if ((await ApplicationData.Current.LocalFolder.TryGetItemAsync("CustomPages")) is null)
            {
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("CustomPages");
            }
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("CustomPages");
            if (await folder.TryGetItemAsync("MainPage.xaml") is StorageFile file1)
            {
                await file1.DeleteAsync();
            }
            await file.CopyAsync(folder, "MainPage.xaml");
            RestartInfoBar.IsOpen = true;
            Common.Settings.EnableCustomPage = true;
            Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackMainPageChanged(file.Name);
        }
    }
}
