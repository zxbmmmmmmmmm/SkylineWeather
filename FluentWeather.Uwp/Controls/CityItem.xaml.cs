using FluentWeather.Abstraction.Models;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls;

public sealed partial class CityItem : UserControl
{
    public CityItem()
    {
        this.InitializeComponent();
    }




    public GeolocationBase Location
    {
        get => (GeolocationBase)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LocationProperty =
        DependencyProperty.Register(nameof(Location), typeof(GeolocationBase), typeof(CityItem), new PropertyMetadata(default));
    





}