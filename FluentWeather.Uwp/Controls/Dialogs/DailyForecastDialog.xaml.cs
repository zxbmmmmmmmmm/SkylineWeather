using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.ViewModels;
using Microsoft.Toolkit.Uwp.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs
{
    public sealed partial class DailyForecastDialog : ContentDialog
    {
        public DailyForecastDialogViewModel ViewModel = new();
        public DailyForecastDialog(ListViewBase listView,object clicked)
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
            ViewModel.Selected = (WeatherBase)clicked;
            foreach (var item in MainPageViewModel.Instance.DailyForecasts)
                ViewModel.DailyForecasts.Add(item);
            foreach(var item in MainPageViewModel.Instance.HourlyForecasts)
                ViewModel.HourlyForecasts.Add(item);
        }
        private bool _isExpanded = false;
        private void ExpandForecastBtnClicked(object sender, RoutedEventArgs e)
        {
            if(_isExpanded)
            {
                Reduce();
            }
            else
            {
                Expand();
            }
            _isExpanded = !_isExpanded;
        }
        private void Expand()
        {
            ForecastRow.Height = new GridLength(1, GridUnitType.Star);
            DetailsRow.Height = new GridLength(0, GridUnitType.Pixel);
            Grid.SetRow(ExpandBtn, 0);
            ForecastGridView.OneRowModeEnabled = false;
            ForecastGridView.MaxWidth = 1120;
            ExpandBtn.VerticalAlignment = VerticalAlignment.Bottom;
            ExpandIcon.Glyph = "\uE010";
            ExpandBtn.CornerRadius = new CornerRadius(4);
            ExpandBtn.BorderThickness = new Thickness(1);
            ExpandText.Text = "折叠";
        }
        private async void Reduce()
        {
            ForecastRow.Height = GridLength.Auto;
            DetailsRow.Height = new GridLength(1, GridUnitType.Star);
            Grid.SetRow(ExpandBtn, 1);
            ForecastGridView.OneRowModeEnabled = true;
            ForecastGridView.MaxWidth = double.MaxValue;
            ExpandBtn.VerticalAlignment = VerticalAlignment.Top;
            ExpandIcon.Glyph = "\uE011";
            ExpandBtn.CornerRadius = new CornerRadius(0, 0, 4, 4);
            ExpandBtn.BorderThickness = new Thickness(1, 0, 1, 1);
            ExpandText.Text = "展开";
            await Task.Delay(10);
            await ForecastGridView.SmoothScrollIntoViewWithItemAsync(ViewModel.Selected,itemPlacement:ScrollItemPlacement.Left);
        }
        private List<ITemperature> GetHourly(ObservableCollection<WeatherBase> weatherList, WeatherBase selected)
        {
            var list = new List<ITime>();
            foreach (var item in weatherList)
            {
                list.Add(item as ITime);
            }
            var time = (ITime)selected;
            var res = list.Where(p => p.Time.Date == time.Time.Date).ToList().ConvertAll(p => (ITemperature)p);
            FullText.Visibility = res.Count is 24 ? Visibility.Visible : Visibility.Collapsed;
            FirstText.Visibility = FullText.Visibility is Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            TemperatureChart.Visibility = res.Count is 0 ? Visibility.Collapsed: Visibility.Visible;
            return res;

        }
        private string GetTextFirst(List<ITemperature> weatherList)
        {
            if (weatherList is null) return null;
            return weatherList.Count is 0 ? null : weatherList?.ConvertAll(p => (ITime)p).ToList().First().Time.ToShortTimeString();
        }
        private string GetTextLast(List<ITemperature> weatherList)
        {
            if (weatherList is null) return null;
            return weatherList.Count is 0 ? null : weatherList?.ConvertAll(p => (ITime)p).ToList().Last().Time.ToShortTimeString();
        }
        private void ForecastGridView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (_isExpanded)
            {
                Reduce();
            }
            else
            {
                Expand();
            }
            _isExpanded = !_isExpanded;
        }
    }
}
