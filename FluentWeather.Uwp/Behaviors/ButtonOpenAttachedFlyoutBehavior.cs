using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonOpenAttachedFlyoutBehavior : Behavior<ButtonBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += OnButtonClicked; ;
    }

    private void OnButtonClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        FlyoutBase.ShowAttachedFlyout(AssociatedObject);
    }
    

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click += OnButtonClicked; ;
    }
}