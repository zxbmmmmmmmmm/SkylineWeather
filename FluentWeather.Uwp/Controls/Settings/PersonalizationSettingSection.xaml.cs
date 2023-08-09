using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Uwp.Behaviors;
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
    }
}
