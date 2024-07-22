using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FluentWeather.Uwp.Behaviors;

public class ListViewScrollBehavior:Behavior<ListViewBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += ListViewLoaded;
    }
    private ScrollViewer _parentScrollViewer;
    private ScrollViewer _scrollViewer;
    private ItemsPresenter _presenter;
    private void ListViewLoaded(object sender, RoutedEventArgs e)
    {
        _scrollViewer = (VisualTreeHelper.GetChild(AssociatedObject, 0) as Border)?.Child as ScrollViewer;
        _presenter = (ItemsPresenter)_scrollViewer?.Content;

        if (_presenter is null) return;
        _parentScrollViewer = AssociatedObject.FindParent<ScrollViewer>();
        _presenter.PointerWheelChanged += WheelChanged;

        if (_scrollViewer is null) return;
        _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
    }

    private void WheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (_scrollViewer.ScrollableWidth == 0) return;
        if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch) return;
        e.Handled = true;
        var delta = e.GetCurrentPoint(sender as UIElement).Properties.MouseWheelDelta;
        _parentScrollViewer?.ChangeView(_parentScrollViewer.HorizontalOffset, _parentScrollViewer.VerticalOffset - delta, 1);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= ListViewLoaded;
        if (_presenter is null) return;
        _presenter.PointerWheelChanged -= WheelChanged;
    }
}