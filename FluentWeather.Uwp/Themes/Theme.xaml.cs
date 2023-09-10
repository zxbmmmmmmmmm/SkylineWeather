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
        var dic = new XamlControlsResources { ControlsResourcesVersion = ControlsResourcesVersion.Version2 };

        switch (Common.Settings.Theme)
        {
            case AppTheme.Fluent:
                dic.Merge("ms-appx:///Themes/DefaultThemeStyles.xaml");
                break;
            case AppTheme.Fluent2017:
                dic.Merge("ms-appx:///Themes/FluentV1ThemeStyles.xaml");
                break;
            case AppTheme.Classic:
                dic.Merge("ms-appx:///Themes/ClassicThemeStyles.xaml");
                break;
        }
        this.Merge(dic);
    }
}
