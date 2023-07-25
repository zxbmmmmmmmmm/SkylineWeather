using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.Weather;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FluentWeather.Uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public async void GetWeather()
        {
            var provider = new QWeatherProvider.QWeatherProvider("b18d888d25b4437cbae4bbf36990092e");
            var jj = await provider.GetCurrentWeather(116, 39);
            var u = jj as ITemperature;
            Activator.CreateInstance(typeof(MainPage), "sss");
            var sc = new ServiceCollection();
            var sp = sc.BuildServiceProvider();
        }
    }
}
