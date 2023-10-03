using FluentWeather.Uwp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using static System.Net.Mime.MediaTypeNames;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentWeather.Uwp.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        private void ToMainPage_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(MainPage));
        }
        private void ToRoot_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(RootPage));
        }
        private void ToCitiesPage_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(CitiesPage));
        }
        
        public void GetLogText()
        {
            //LogText.Text = Common.LogManager.GetLogger("").
        }

        private async void AppFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            //Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
            try
            {
                var logFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("MetroLogs");
                var files =await logFolder.GetItemsAsync();
                LogsView.ItemsSource = files;
            }
            catch(Exception ex)
            {
                LogText.Text = ex.Message;
            }
        }

        private async void LogsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogsView.SelectedItem is null) return;
            try
            {
                string text = await Windows.Storage.FileIO.ReadTextAsync((IStorageFile)e.AddedItems[0]);
                LogText.Text = text;
            }
            catch (Exception ex)
            {
                Launcher.LaunchFileAsync((IStorageFile)e.AddedItems[0]);
                LogText.Text = ex.Message;
            }
        }

        private async void CleanLogsBtn_Click(object sender, RoutedEventArgs e)
        {
            var logFolder = await ApplicationData.Current.LocalFolder.TryGetItemAsync("MetroLogs") as StorageFolder;
            logFolder?.DeleteAsync();
        }

        private async void GetLogBtn1_Click(object sender, RoutedEventArgs e)
        {
            var logFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("MetroLogs");
            var files = await logFolder.GetItemsAsync();
            string text = await Windows.Storage.FileIO.ReadTextAsync((IStorageFile)files[0]);
            LogText.Text = text;
        }
    }
}
