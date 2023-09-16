using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Uwp.Behaviors;
using FluentWeather.Uwp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        private readonly string backgroundImageSettingInfo = "你也可以根据不同天气使用不同的背景\r\n\r\n方法:点击\"打开背景文件夹\"，将背景图片**(PNG格式)**复制到此文件夹内，将图片按照以下格式重命名即可:\r\n\r\n|  天气   | 文件命名              |\r\n|-------|-------------------|\r\n| 晴     | Clear             |\r\n| 多云    | PartlyCloudy      |\r\n| 阴     | Cloudy            |\r\n| 大雨    | HeavyRain         |\r\n| 小雨/中雨 | LightRain         |\r\n| 大雪    | HeavySnow         |\r\n| 小雪/中雪 | LightSnow         |\r\n| 雷阵雨   | ThunderyShowers   |\r\n| 雷电    | ThunderyHeavyRain |\r\n| 雾/霾   | Fog             ";
        [RelayCommand]
        public async Task SetBackground(string type)
        {
            var imageFile = await PickFileAsync();
            if (imageFile is null) return;
            if ((await ApplicationData.Current.LocalFolder.TryGetItemAsync("Backgrounds")) is null)
            {
                ApplicationData.Current.LocalFolder.CreateFolderAsync("Backgrounds");
            }
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Backgrounds");
            if(await folder.TryGetItemAsync(type + ".png") is StorageFile file)
            {
                await file.DeleteAsync();
            }
            await imageFile.CopyAsync(folder, type + ".png");
            LoadLocalBackgroundBehavior.Instance?.LoadImage();
        }
        [RelayCommand]
        public async Task OpenBackgroundsFolder()
        {
            if ((await ApplicationData.Current.LocalFolder.TryGetItemAsync("Backgrounds")) is null)
            {
                ApplicationData.Current.LocalFolder.CreateFolderAsync("Backgrounds");
            }
            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Backgrounds");
            Launcher.LaunchFolderAsync(folder);
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
    }
}
