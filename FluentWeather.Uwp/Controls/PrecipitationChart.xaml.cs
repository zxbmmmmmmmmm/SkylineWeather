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

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class PrecipitationChart : UserControl
    {
        public PrecipitationChart()
        {
            this.InitializeComponent();
        }

        public List<PrecipitationItemBase> Precipitations
        {
            get => (List<PrecipitationItemBase>)GetValue(PrecipitationsProperty);
            set => SetValue(PrecipitationsProperty, value);
        }
        public static readonly DependencyProperty PrecipitationsProperty =
            DependencyProperty.Register(nameof(Precipitations), typeof(List<PrecipitationItemBase>), typeof(PrecipitationChart), new PropertyMetadata(default));
    }
}
