using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FluentWeather.Uwp.Controls.Settings
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DataProviderSettingSection : Page
    {
        public DataProviderSettingSection()
        {
            this.InitializeComponent();
            //if (Equals(Common.Settings.DataProviderConfig, DataProviderHelper.QWeatherConfig))
            //    DataProviderComboBox.SelectedIndex = 0;
            //if (Equals(Common.Settings.DataProviderConfig, DataProviderHelper.OpenMeteoConfig))
            //    DataProviderComboBox.SelectedIndex = 1;
        }
        //public static bool Equals<TKey, TValue>(IList<KeyValuePair<TKey, TValue>> x,
        //    IList<KeyValuePair<TKey, TValue>> y)
        //{
        //    return x.All(p => y.Any(r => r.Equals(p))) && x.Count == y.Count;
        //}
        //private void DataProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{           
        //    if (e.AddedItems?[0] is "和风天气")
        //        Common.Settings.DataProviderConfig = DataProviderHelper.QWeatherConfig; 
        //    if (e.AddedItems?[0] is "Open-Meteo")
        //        Common.Settings.DataProviderConfig = DataProviderHelper.OpenMeteoConfig;
        //}
    }
}
