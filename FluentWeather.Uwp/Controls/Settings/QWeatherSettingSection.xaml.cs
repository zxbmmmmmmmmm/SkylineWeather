using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls.Settings;

[ObservableObject]
public sealed partial class QWeatherSettingSection : UserControl
{
    public QWeatherSettingSection()
    {
        this.InitializeComponent();
        //var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        //var settings = Locator.ServiceProvider.GetService<ISetting>();
        //Token = settingsHelper.ReadLocalSetting(settings.Id + "." + nameof(Token),"");
        //PropertyChanged += OnPropertyChanged; 
    }

    //private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
    //    var settings = Locator.ServiceProvider.GetService<ISetting>();
    //    //Token = settingsHelper.ReadLocalSetting(settings.Id + "." + nameof(Token), "");

    //    switch (e.PropertyName)
    //    {
    //        case nameof(Token):
    //            settingsHelper.WriteLocalSetting(settings.Id + "." + nameof(Token), Token);
    //            break;
    //    }
    //}

    //[ObservableProperty]
    //private string _token;


}

