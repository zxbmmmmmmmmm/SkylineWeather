using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Shared;

public class ThemeHelper
{
    private static ElementTheme _theme;
    public static async Task SetRequestedThemeAsync(ElementTheme theme)
    {
        foreach (var view in CoreApplication.Views)
        {
            await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = theme;
                }
            });
        }
    }
    public static async void SetRequestTheme(ElementTheme theme)
    {
        if (_theme == theme) return;
        _theme = theme;
        await SetRequestedThemeAsync(theme);
        SetTitleBarColor(theme);
    }
    public static void SetTitleBarColor(ElementTheme theme)
    {
        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
        switch (theme)
        {
            case ElementTheme.Light:
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                break;
            case ElementTheme.Dark:
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverForegroundColor = Colors.White;
                break;
            case ElementTheme.Default:
                titleBar.ButtonForegroundColor = default;
                titleBar.ButtonHoverForegroundColor = default;
                break;
        }

    }
}