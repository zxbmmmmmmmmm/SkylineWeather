using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Media;
using System.ServiceModel.Channels;
using Microsoft.Toolkit.Uwp.UI.Converters;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonListViewScrollBehavior: Behavior<Button>
{


    public ListViewBase ListView
    {
        get => (ListViewBase)GetValue(ListViewProperty);
        set => SetValue(ListViewProperty, value);
    }
    public static readonly DependencyProperty ListViewProperty =
        DependencyProperty.Register(nameof(ListView), typeof(ListViewBase), typeof(ButtonListViewScrollBehavior), new PropertyMetadata(null));


    public bool IsRight
    {
        get => (bool)GetValue(IsRightProperty);
        set => SetValue(IsRightProperty, value);
    }

    public static readonly DependencyProperty IsRightProperty =
        DependencyProperty.Register(nameof(IsRight), typeof(bool), typeof(ButtonListViewScrollBehavior), new PropertyMetadata(true));
    private ScrollViewer _listScrollViewer;
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += ButtonClicked;
        AssociatedObject.Loaded += OnLoaded;

    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _listScrollViewer = (VisualTreeHelper.GetChild(ListView, 0) as Border)?.Child as ScrollViewer;
        if (_listScrollViewer is null) return;
        _listScrollViewer.ViewChanged += OnScrollViewChanged;
        _listScrollViewer.LayoutUpdated += (s, e) => OnScrollViewChanged(null, null);
    }

    private void ButtonClicked(object sender, RoutedEventArgs e)
    {
        if(IsRight)
        {
            _listScrollViewer?.ChangeView(_listScrollViewer.HorizontalOffset + ListView.ActualWidth, 0, 1);
        }
        else
        {
            _listScrollViewer?.ChangeView(_listScrollViewer.HorizontalOffset - ListView.ActualWidth, 0, 1);
        }
    }

    private void OnScrollViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        if(AssociatedObject is null) return;
        AssociatedObject.Visibility = (_listScrollViewer.ScrollableWidth > 0) ? Visibility.Visible : Visibility.Collapsed;

        AssociatedObject.IsEnabled = CanScroll(_listScrollViewer);
    }

    private bool CanScroll(ScrollViewer scroll)
    {

        var dHor = scroll.HorizontalOffset;
        if (!IsRight)
        {
            return dHor is not 0;
        }
        var dViewport = scroll.ViewportWidth;
        var dExtent = scroll.ExtentWidth;
        if (dHor == 0) return true;
        return dHor + dViewport != dExtent;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching(); 
        AssociatedObject.Click -= ButtonClicked;
        _listScrollViewer.ViewChanged -= OnScrollViewChanged;
        _listScrollViewer.LayoutUpdated -= (s, e) => OnScrollViewChanged(null, null);
    }
}