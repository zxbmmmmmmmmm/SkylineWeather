using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentWeather.Uwp.Controls;

public partial class LinearGauge
{
    private const double DefaultMinimum = 0.0;
    private const double DefaultMaximum = 10.0;
    /// <summary>
    /// Identifies the <see cref="Minimum"/> property.
    /// </summary>
    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(
            nameof(Minimum),
            typeof(double),
            typeof(LinearGauge),
            new PropertyMetadata(DefaultMinimum, MinimumChangedCallback));

    /// <summary>
    /// Identifies the <see cref="Maximum"/> property.
    /// </summary>
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(
            nameof(Maximum),
            typeof(double),
            typeof(LinearGauge),
            new PropertyMetadata(DefaultMaximum, MaximumChangedCallback));


    /// <summary>
    /// Identifies the <see cref="RangeStart"/> property.
    /// </summary>
    public static readonly DependencyProperty RangeStartProperty =
        DependencyProperty.Register(
            nameof(RangeStart),
            typeof(double),
            typeof(LinearGauge),
            new PropertyMetadata(DefaultMinimum, RangeMinChangedCallback));

    /// <summary>
    /// Identifies the <see cref="RangeEnd"/> property.
    /// </summary>
    public static readonly DependencyProperty RangeEndProperty =
        DependencyProperty.Register(
            nameof(RangeEnd),
            typeof(double),
            typeof(LinearGauge),
            new PropertyMetadata(DefaultMaximum, RangeMaxChangedCallback));


    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Gets or sets the absolute maximum value of the range.
    /// </summary>
    /// <value>
    /// The maximum.
    /// </value>
    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Gets or sets the current selected lower limit value of the range, modifiable by the user.
    /// </summary>
    /// <value>
    /// The current lower limit.
    /// </value>
    public double RangeStart
    {
        get => (double)GetValue(RangeStartProperty);
        set => SetValue(RangeStartProperty, value);
    }

    /// <summary>
    /// Gets or sets the current selected upper limit value of the range, modifiable by the user.
    /// </summary>
    /// <value>
    /// The current upper limit.
    /// </value>
    public double RangeEnd
    {
        get => (double)GetValue(RangeEndProperty);
        set => SetValue(RangeEndProperty, value);
    }
    private static void MinimumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var rangeSelector = d as LinearGauge;

        if (rangeSelector == null)
        {
            return;
        }

        rangeSelector.SyncActiveRectangle();
    }

    private static void MaximumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var rangeSelector = d as LinearGauge;

        if (rangeSelector == null)
        {
            return;
        }

        rangeSelector.SyncActiveRectangle();
    }

    private static void RangeMinChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var rangeSelector = d as LinearGauge;

        if (rangeSelector == null)
        {
            return;
        }

        rangeSelector.SyncActiveRectangle();
    }

    private static void RangeMaxChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var rangeSelector = d as LinearGauge;

        if (rangeSelector == null)
        {
            return;
        }

        rangeSelector.SyncActiveRectangle();
    }
}