using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CommunityToolkit.WinUI;
using FluentWeather.Uwp.Pages;
using Windows.UI.Xaml.Media;

namespace FluentWeather.Uwp.Behaviors;

public sealed class SetParallaxViewSourceBehavior : Behavior<FrameworkElement>
{
    private ScrollViewer _scrollViewer;





    public int HorizontalShift
    {
        get => (int)GetValue(HorizontalShiftProperty);
        set => SetValue(HorizontalShiftProperty, value);
    }

    public static readonly DependencyProperty HorizontalShiftProperty =
        DependencyProperty.Register(nameof(HorizontalShift), typeof(int), typeof(SetParallaxViewSourceBehavior), new PropertyMetadata(0));

    public int VerticalShift
    {
        get => (int)GetValue(VerticalShiftProperty);
        set => SetValue(VerticalShiftProperty, value);
    }

    public static readonly DependencyProperty VerticalShiftProperty =
        DependencyProperty.Register(nameof(VerticalShift), typeof(int), typeof(SetParallaxViewSourceBehavior), new PropertyMetadata(0));


    protected override void OnAttached()
    {
        AssociatedObject.Loaded += OnLoaded;
        base.OnAttached();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var viewer = AssociatedObject as ScrollViewer;
        viewer ??= FindScrollViewer(AssociatedObject);
        _scrollViewer = viewer;
        RootPage.Instance.BackgroundParallaxView.Source = _scrollViewer;
        RootPage.Instance.BackgroundParallaxView.HorizontalShift = HorizontalShift;
        RootPage.Instance.BackgroundParallaxView.VerticalShift = VerticalShift;
    }

    protected override void OnDetaching()
    {
        RootPage.Instance.BackgroundParallaxView.Source = null;
        AssociatedObject.Loaded -= OnLoaded;
        base.OnDetaching();
    }



    private static ScrollViewer FindScrollViewer(FrameworkElement element)
    {
        var viewer = element.FindChild<ScrollViewer>();
        viewer ??= (VisualTreeHelper.GetChild(element, 0) as FrameworkElement)?.FindChild<ScrollViewer>();
        return viewer;
    }
}