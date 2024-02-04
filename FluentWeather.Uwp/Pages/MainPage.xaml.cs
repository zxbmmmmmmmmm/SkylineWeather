using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.ViewModels;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Effects;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using FluentWeather.Uwp.Helpers;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentWeather.Uwp.Pages;
public sealed partial class MainPage : Page
{
    public MainPageViewModel ViewModel { get; set; } = new();
    public static MainPage Instance ;
    private XamlRenderService _xamlRenderer = new XamlRenderService();
    public MainPage()
    {
        this.DataContext = ViewModel;
        _xamlRenderer.DataContext = ViewModel;
        Instance = this;
        LoadElements();
    }

    private void DailyItemClicked(object sender, ItemClickEventArgs e)
    {
        //DailyView.SelectedIndex = DailyGridView.IndexFromContainer(DailyGridView.ContainerFromItem(e.ClickedItem));
        Analytics.TrackEvent("DailyViewEntered");
    }

    private Visibility GetPrecipChartVisibility(PrecipitationBase precip)
    {
        var precipList = precip?.Precipitations;
        if (precipList is null) return Visibility.Collapsed;
        if (precipList.Count is 0) return Visibility.Collapsed;
        return precipList.Sum(p => p.Precipitation) == 0 ? Visibility.Collapsed : Visibility.Visible;
    }

    private async void LoadElements()
    {
        var content = await GetCustomContent();
        if (content is null)
        {
            this.InitializeComponent();
            //DailyGridView.ItemClick += DailyItemClicked;
            MainPageViewModel.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName is not "CurrentLocation") return;
                //MainContentContainer.Visibility = Visibility.Visible;
            };
            return; 
        }
        this.Content = content;
    }
    
    private async Task<UIElement> GetCustomContent()
    {
        StorageFolder folder;
        try
        {
            folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("CustomPages");
        }
        catch
        {
            folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Backgrounds");
        }
        try
        {
            var file = await folder.GetFileAsync("MainPage.xaml");
            var text = await FileIO.ReadTextAsync(file);
            return _xamlRenderer.Render(text);
        }
        catch
        {
            return null;
        }
    }
}