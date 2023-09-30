using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Themes;

public partial class Theme:ResourceDictionary
{
    public Theme()
    {
        this.InitializeComponent();
        var dic = new ResourceDictionary();
        //switch (Common.Settings.Theme)
        //{
        //    case AppTheme.Fluent:
        //        dic.MergeMUXC(ControlsResourcesVersion.Version2);
        //        break;
        //    case AppTheme.Fluent2017:
        //        dic.MergeMUXC(ControlsResourcesVersion.Version1);
        //        break;
        //    case AppTheme.Classic:
        //        dic.MergeMUXC(ControlsResourcesVersion.Version1);
        //        break;
        //}
        dic.MergedDictionaries.Add(new XamlControlsResources());
        dic.Merge("ms-appx:///Styles/ListView.xaml");
        dic.Merge("ms-appx:///Styles/ContentDialog.xaml");
        dic.Merge("ms-appx:///Themes/Generic.xaml");

        //switch (Common.Settings.Theme)
        //{
        //    case AppTheme.Fluent:
        //        dic.Merge("ms-appx:///Themes/DefaultThemeStyles.xaml");
        //        break;
        //    case AppTheme.Fluent2017:
        //        dic.Merge("ms-appx:///Themes/FluentV1ThemeStyles.xaml");
        //        break;
        //    case AppTheme.Classic:
        //        dic.Merge("ms-appx:///Themes/ClassicThemeStyles.xaml");
        //        break;
        //}
        this.Merge(dic);
    }
}
