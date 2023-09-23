using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Themes;

public partial class Theme:ResourceDictionary
{
    public Theme()
    {
        this.InitializeComponent();
        var dic = new ResourceDictionary();

        switch (Common.Settings.Theme)
        {
            case AppTheme.Fluent:
                dic.MergeMUXC(ControlsResourcesVersion.Version2);
                dic.Merge("ms-appx:///Themes/DefaultThemeStyles.xaml");
                break;
            case AppTheme.Fluent2017:
                dic.MergeMUXC(ControlsResourcesVersion.Version1);
                dic.Merge("ms-appx:///Themes/FluentV1ThemeStyles.xaml");
                break;
            case AppTheme.Classic:
                dic.MergeMUXC(ControlsResourcesVersion.Version1);
                dic.Merge("ms-appx:///Themes/ClassicThemeStyles.xaml");
                break;
        }
        this.Merge(dic);
    }
}
