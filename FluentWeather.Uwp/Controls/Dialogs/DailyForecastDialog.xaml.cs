using FluentWeather.Abstraction.Models;
using FluentWeather.Uwp.ViewModels;
using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
