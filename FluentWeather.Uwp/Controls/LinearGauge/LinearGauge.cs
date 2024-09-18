using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace FluentWeather.Uwp.Controls;

[TemplateVisualState(Name = NormalState, GroupName = CommonStates)]
[TemplatePart(Name = "ContainerCanvas", Type = typeof(Canvas))]
[TemplatePart(Name = "ActiveRectangle", Type = typeof(Border))]
[TemplateVisualState(Name = DisabledState, GroupName = CommonStates)]
public sealed partial class LinearGauge : Control
{
    internal const string CommonStates = "CommonStates";

    internal const string NormalState = "Normal";
    internal const string DisabledState = "Disabled";

    private Canvas _containerCanvas;
    private Border _activeRectangle;
    private bool _minSet;
    private bool _maxSet;
    private bool _valuesAssigned;

    public LinearGauge()
    {
        this.DefaultStyleKey = typeof(LinearGauge);
    }
    protected override void OnApplyTemplate()
    {

        if (_containerCanvas != null)
        {
            _containerCanvas.SizeChanged -= ContainerCanvas_SizeChanged;
        }

        IsEnabledChanged -= RangeSelector_IsEnabledChanged;

        // Need to make sure the values can be set in XAML and don't overwrite each other
        VerifyValues();
        _valuesAssigned = true;

        _activeRectangle = GetTemplateChild("ActiveRectangle") as Border;

        _containerCanvas = GetTemplateChild("ContainerCanvas") as Canvas;


        if (_containerCanvas != null)
        {
            _containerCanvas.SizeChanged += ContainerCanvas_SizeChanged;
        }

        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, false);

        IsEnabledChanged += RangeSelector_IsEnabledChanged;

        base.OnApplyTemplate();
    }

    private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        SyncActiveRectangle();
    }
    private void RangeSelector_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        VisualStateManager.GoToState(this, IsEnabled ? NormalState : DisabledState, true);
    }

    private void SyncActiveRectangle()
    {
        if (_containerCanvas == null)
        {
            return;
        }
        if (_activeRectangle == null)
        {
            return;
        }
        var relativeLeft = ((RangeStart - Minimum) / (Maximum - Minimum)) * ActualWidth ;
        var relativeRight = ((RangeEnd - Minimum) / (Maximum - Minimum)) * ActualWidth;
        Canvas.SetLeft(_activeRectangle, relativeLeft);
        Canvas.SetTop(_activeRectangle, (_containerCanvas.ActualHeight - _activeRectangle!.ActualHeight) / 2);
        _activeRectangle.Width = Math.Max(0, relativeRight - relativeLeft);
    }
    private void VerifyValues()
    {
        if (Minimum > Maximum)
        {
            Minimum = Maximum;
            Maximum = Maximum;
        }

        //if (!_maxSet)
        //{
        //    RangeEnd = Maximum;
        //}

        //if (!_minSet)
        //{
        //    RangeStart = Minimum;
        //}

        if (RangeStart < Minimum)
        {
            RangeStart = Minimum;
        }

        if (RangeEnd < Minimum)
        {
            RangeEnd = Minimum;
        }

        if (RangeStart > Maximum)
        {
            RangeStart = Maximum;
        }

        if (RangeEnd > Maximum)
        {
            RangeEnd = Maximum;
        }

        if (RangeEnd < RangeStart)
        {
            RangeStart = RangeEnd;
        }
    }

}