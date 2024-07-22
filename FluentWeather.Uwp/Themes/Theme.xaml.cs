using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace FluentWeather.Uwp.Themes;

public sealed partial class Theme:ResourceDictionary
{
    public Theme()
    {
        this.InitializeComponent();
        var dic = new ResourceDictionary();
        switch (Common.Settings.Theme)
        {
            case AppTheme.Fluent:
                dic.MergeMUXC(ControlsResourcesVersion.Version2);
                break;
            case AppTheme.Fluent2017:
                dic.MergeMUXC(ControlsResourcesVersion.Version1);
                break;
            case AppTheme.Classic:
                dic.MergeMUXC(ControlsResourcesVersion.Version1);
                break;
        }

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
    public static NavigationTransitionInfo GetNavigationTransition()
    {
        return Common.Settings.Theme switch
        {
            AppTheme.Fluent => new DrillInNavigationTransitionInfo(),
            AppTheme.Fluent2017 => new DrillInNavigationTransitionInfo(),
            AppTheme.Classic => new CommonNavigationTransitionInfo(),
            _ => new EntranceNavigationTransitionInfo()
        };
    }
    public static NavigationTransitionInfo GetSplitPaneNavigationTransition()
    {
        return Common.Settings.Theme switch
        {
            AppTheme.Fluent => new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            },
            AppTheme.Fluent2017 => new SlideNavigationTransitionInfo()
            {
                Effect = SlideNavigationTransitionEffect.FromRight
            },
            AppTheme.Classic => new CommonNavigationTransitionInfo(),
            _ => new EntranceNavigationTransitionInfo()
        };
    }
}
